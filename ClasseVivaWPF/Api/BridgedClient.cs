using ClasseVivaWPF.Api.Types;
using ClasseVivaWPF.Utils.Logs;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ClasseVivaWPF.Api
{
    public partial class Client
    {
        public const string WEB_APP_BASE = "https://web.spaggiari.eu/tic/app/default/";

        public static readonly (string Code, string Desc)[] AllowedGiustifications = new[] { 
            ("", "Nessuno"),
            ("A", "Salute"),
            ("AC", "Certificato Medico"),
            ("B", "Famiglia"),
            ("C", "Altro"),
            ("D", "Trasporto"),
            ("E", "Sciopero"),
        };
        public static string[] AllowedGiustificationsCodes => AllowedGiustifications.Select(x => x.Code).ToArray();

        private string BuildUrl(string path, JObject? @params)
        {
            var url = WEB_APP_BASE + path;
            if (@params is null)
                return url;

            var query_builder = HttpUtility.ParseQueryString("");
            foreach (var param in @params)
                query_builder.Add(param.Key, param.Value!.ToString());

            return url + "?" + query_builder.ToString()!;
        }

        private async Task<bool> OpenUrl(Uri url)
        {
            try
            {
                var res = await this.client.GetAsync(url);
                if (!res.IsSuccessStatusCode)
                {
                    if (Logger.CanLog(LogLevel.TRACE))
                        Logger.Log($"Call to {url} failed with {(int)res.StatusCode} {res.ReasonPhrase}\n{res.Content.ReadAsStringAsync().Result}", LogLevel.INFO);
                    else if (Logger.CanLog(LogLevel.INFO))
                        Logger.Log($"Call to {url} failed with {(int)res.StatusCode} {res.ReasonPhrase}", LogLevel.INFO);
                }

                return res.IsSuccessStatusCode;
            }catch (Exception ex)
            {
                Logger.Log($"Call to {url} failed due:\n{ex.Message}", LogLevel.INFO);
                return false;
            }
        }

        public async Task<bool> BridgedGiustifyAbsence(Event absence, string reason_code, string reason, string? origin = null)
        {
            Debug.Assert(!absence.IsEarlyExit);
            Debug.Assert(origin is null);
            Debug.Assert(AllowedGiustificationsCodes.Contains(reason_code));
            origin = "";

            var @event = absence.IsLate ? "R" : absence.IsAbsence ? "A" : "U";

            var req = new JObject()
            {
                { "studente", this.Me.LastName + "+" + this.Me.FirstName },
                { "studente_id", this.Me.Id },
                { "data_inizio", absence.EvtDate.ToString("yyyy-MM-dd") },
                { "data_fine", absence.EvtDate.AddHours(absence.HoursAbsence.LastOrDefault(0)).ToString("yyyy-MM-dd") },
                { "evento", @event },
                { "data", DateTime.Now.ToString("yyyy-MM-dd") },
                { "id_ori", absence.EvtId },
                { "origin", origin },
                { "new_evento", @event },
                { "ope", "INS" },
                { "causale", reason_code },
                { "annotazione", reason },
            };

            var target_url = BuildUrl("regassenzeins_giu.php", req);
            var url = await GetUriFromTicket(target_url);
            
            return await OpenUrl(url);
        }
    }
}
