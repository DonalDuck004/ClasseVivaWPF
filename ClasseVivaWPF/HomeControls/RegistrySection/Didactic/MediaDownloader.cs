using ClasseVivaWPF.Api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ClasseVivaWPF.HomeControls.RegistrySection.Didactic
{
    public class MediaDownloader : DependencyObject
    {
        public const int BUFF_SIZE = 0xFFF;

        public static readonly DependencyProperty StartedProperty;
        public static readonly DependencyProperty WroteProperty;
        public static readonly DependencyProperty TotalProperty;
        private static Dictionary<int, MediaDownloader> Downloaders = new();

        public bool Started
        {
            get => (bool)GetValue(StartedProperty);
            set => SetValue(StartedProperty, value);
        }

        public int Wrote
        {
            get => (int)GetValue(WroteProperty);
            set => SetValue(WroteProperty, value);
        }

        public int Total
        {
            get => (int)GetValue(TotalProperty);
            set => SetValue(TotalProperty, value);
        }

        private SemaphoreSlim sem = new SemaphoreSlim(1, 1);
        private bool _started = false;

        private int id;
        private Task? DownloadTask = null;
        
        public string Name { get; private set; }


        public delegate void ProgressHandler(MediaDownloader sender);
        public event ProgressHandler Progess;

        public Action<MediaDownloader>? DownloadCompleted = null;
        public Action<MediaDownloader, Task, Stream>? DownloadTaskEnded = null;

        static MediaDownloader()
        {
            StartedProperty = DependencyProperty.Register("Started", typeof(bool), typeof(MediaDownloader), new PropertyMetadata(false));
            WroteProperty = DependencyProperty.Register("Wrote", typeof(int), typeof(MediaDownloader), new PropertyMetadata(0));
            TotalProperty = DependencyProperty.Register("Total", typeof(int), typeof(MediaDownloader), new PropertyMetadata(0));
        }

        private MediaDownloader(int id)
        {
            this.id = id;
        }

        public static MediaDownloader New(int id)
        {
            lock(Downloaders)
            {
                if (!Downloaders.ContainsKey(id))
                    Downloaders[id] = new(id);

                return Downloaders[id];
            }
        }

        private void RaiseDownloadTaskEnded(Task result, Stream stream)
        {
            if (DownloadTaskEnded is not null)
                DownloadTaskEnded(this, result, stream);
        }

        private void RaiseProgress()
        {
            if (Progess is not null)
                Progess(this);

            if (this.Wrote == this.Total)
                RaiseDownloadCompleted();
        }

        private void RaiseDownloadCompleted()
        {
            if (DownloadCompleted is not null)
                DownloadCompleted(this);
        }

        private async void DownloadHandler(Stream stream)
        {
            try
            {
                var url = $"{Client.ENDPOINT}rest/v1/students/{Client.INSTANCE.UserID}/didactics/item/{this.id}";
                var reply = await Client.INSTANCE.CVClient.SendAsync(new HttpRequestMessage(HttpMethod.Head, url));
                this.Name = Path.GetFileName(reply.RequestMessage!.RequestUri!.AbsolutePath);

                var last_call = this.Dispatcher.BeginInvoke(() => { this.Started = true; this.Total = (int)reply.Content.Headers.ContentLength!; this.RaiseProgress(); });

                int chunk_len;
                var buff = new byte[BUFF_SIZE];
                var src = await Client.INSTANCE.CVClient.GetStreamAsync(url);

                while ((chunk_len = await src.ReadAsync(buff, 0, BUFF_SIZE)) != 0)
                {
                    last_call.Wait();
                    await stream.WriteAsync(buff, 0, chunk_len);
                    last_call = this.Dispatcher.BeginInvoke((object x) => { 
                        this.Wrote += (int)x; 
                        RaiseProgress();
                    }, chunk_len);
                }

                last_call.Wait();
            }
            finally
            {
                this.Dispatcher.Invoke(() => this.RaiseDownloadTaskEnded(DownloadTask!, stream));
            }
        }

        public async Task<bool> Begin(Stream stream)
        {
            await sem.WaitAsync();
            if (this._started)
                return false;

            this._started = true;

            sem.Release();

            if (DownloadTask is not null)
                return false;

            DownloadTask = Task.Run(() => DownloadHandler(stream));

            return true;
        }

        public void Reset()
        {
            this.Wrote = 0;
            this.Total = 0;
            this.Started = false;
        }
    }
}
