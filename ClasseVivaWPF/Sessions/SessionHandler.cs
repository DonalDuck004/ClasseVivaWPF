using ClasseVivaWPF.Api;
using ClasseVivaWPF.Api.Types;
using ClasseVivaWPF.Utils;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.ConstrainedExecution;
using System.Threading;

namespace ClasseVivaWPF.Sessions
{
    public class SessionHandler
    {
        public delegate void NotificationsFlagHandler(SessionHandler sender, bool Flag);
        public delegate void NotificationsRangeHandler(SessionHandler sender, int Range);
        public delegate void PagesStackSizeHandler(SessionHandler sender, int Size);
        public NotificationsFlagHandler? NotificationsFlagChanged = null;
        public NotificationsRangeHandler? NotificationsRangeChanged = null;
        public PagesStackSizeHandler? PagesStackSizeChanged = null;
        
        public static SessionHandler? INSTANCE { get; private set; }

        private static Me? _me = null;
        public static Me? Me
        {
            get => _me;
            private set
            {
                _me = value;
                if (value is not null)
                    Client.INSTANCE!.SetLoginToken(value.Token);
            }
        }

        public static bool Logged => Me is not null;

        public string FileName { get; private init; }
        private SqliteConnection conn { get; set; }
        
        private SessionHandler(string session_name)
        {
            this.FileName = session_name;

            var exists = File.Exists(this.FileName);
            this.conn = new SqliteConnection($"Data Source={this.FileName}");
            this.conn.Open();
            if (!exists)
                this.LoadSchema();
        }

        public static bool TryInit(out string? api_error_message, int? idx = null)
        {
            api_error_message = null;
            var current = SessionMetaController.Current;

            if (current.HasAccounts)
            {
                var @this = new SessionHandler(SessionFileFor((idx is null ? current.CurrentAccount : current.Accounts[idx.Value])!.Ident));
                try
                {
                    @this.GetMe();
                }
                catch (InvalidDataException e)
                {
                    if (e.InnerException is ApiError ae)
                        api_error_message = ae.Message;

                    @this.Destroy();
                    return false;
                }

                SessionHandler.INSTANCE = @this;
                return true;
            }

            return false;
        }

        public static string SessionFileFor(string ident)
        {
            return Path.Join(Config.SESSIONS_DIR_PATH, ident) + ".db";
        }

        public static bool ExistsSessionFor(string ident)
        {
            return File.Exists(SessionFileFor(ident));
        }

        public static SessionHandler InitConn(string ident)
        {
            return SessionHandler.INSTANCE = new(SessionFileFor(ident));
        }

        public void LoadSchema()
        {
            var sqls = new string[]
            {
                "CREATE TABLE Session(ident VARCHAR(16) PRIMARY KEY, firstName VARCHAR(48) NOT NULL, lastName VARCHAR(48) NOT NULL, showPwdChangeReminder BOOL NOT NULL, token VARCHAR(512) NOT NULL, release VARCHAR(25) NOT NULL, expire VARCHAR(25) NOT NULL, uid VARCHAR(128) NOT NULL, pass NOT NULL)",
                "CREATE TABLE RequestsCache(url_path VARCHAR(256) PRIMARY KEY, response TEXT, update_date VARCHAR(32) NOT NULL, etag VARCHAR(32))",
                "CREATE TABLE Settings(NotificationsEnabled BOOL DEFAULT True, NotificationsRange INT DEFAULT 6, PagesStackSize INT DEFAULT 5, Year INT DEFAULT NULL)",
                "CREATE TABLE MappedFiles(MediaID int PRIMARY KEY, FileName TEXT)",
                "INSERT INTO Settings DEFAULT VALUES"
            };
            foreach (var sql in sqls)
            {
                using (var cur = this.conn.CreateCommand())
                {
                    cur.CommandText = sql;
                    cur.ExecuteNonQuery();
                }
            }
        }

        public void AddMappedFile(int id, string fname)
        {
            var sql = "INSERT OR REPLACE INTO MappedFiles(MediaID, FileName) VALUES ($id, $fname)";
            using (var cur = this.conn.CreateCommand())
            {
                cur.CommandText = sql;
                cur.Parameters.AddWithValue("$id", id);
                cur.Parameters.AddWithValue("$fname", fname);
                cur.ExecuteNonQuery();
            }
        }

        public string? GetMappedFile(int id)
        {
            var sql = "SELECT FileName FROM MappedFiles WHERE MediaID = $id";
            using (var cur = this.conn.CreateCommand())
            {
                cur.CommandText = sql;
                cur.Parameters.AddWithValue("$id", id);
                using (var row = cur.ExecuteReader())
                {
                    if (row.Read())
                        return row.IsDBNull(0) ? null : row.GetString(0);
                }
            }

            return null;
        }

        public DateTime? CheckCache(string url_path, out string? response, out string? etag)
        {
            var sql = "SELECT response, etag, update_date FROM RequestsCache WHERE url_path = $url_path";
            using (var cur = this.conn.CreateCommand())
            {
                cur.CommandText = sql;
                cur.Parameters.AddWithValue("$url_path", url_path);
                using (var row = cur.ExecuteReader())
                {
                    if (row.Read())
                    {
                        response = row.GetString(0);
                        etag = row.IsDBNull(1) ? null : row.GetString(1);
                        return row.GetDateTime(2);
                    }
                }

            }

            etag = response = null;
            return null;
        }

        public void SetCache(string url_path, string response, string? etag)
        {
            var sql = "INSERT OR REPLACE INTO RequestsCache(url_path, response, update_date, etag) VALUES ($url_path, $response, DATETIME('now'), $etag)";
            using (var cur = this.conn.CreateCommand())
            {
                cur.CommandText = sql;
                cur.Parameters.AddWithValue("$url_path", url_path);
                cur.Parameters.AddWithValue("$response", response);
                cur.Parameters.AddWithValue("$etag", etag is null ? DBNull.Value : etag);
                cur.ExecuteNonQuery();
            }
        }

        public void Destroy()
        {
            var sql = "DELETE FROM Session";
            using (var cur = this.conn.CreateCommand())
            {
                cur.CommandText = sql;
                cur.ExecuteNonQuery();
            }
        }

        public Me GetMe()
        {
            var sql = "SELECT ident, firstName, lastName, showPwdChangeReminder, token, release, expire FROM Session";
            using (var cur = this.conn.CreateCommand())
            {
                cur.CommandText = sql;
                using (var result = cur.ExecuteReader())
                {
                    if (!result.Read())
                        throw new InvalidDataException("No Session Is Active");

                    SessionHandler.Me = new()
                    {
                        Ident = result.GetString(0),
                        FirstName = result.GetString(1),
                        LastName = result.GetString(2),
                        ShowPwdChangeReminder = result.GetBoolean(3),
                        Token = result.GetString(4),
                        Release = result.GetDateTime(5),
                        Expire = result.GetDateTime(6)
                    };
                }
            }

            try
            {
                this.RenewToken(SessionHandler.Me.Expire);
            }catch (ApiError e)
            {
                throw new InvalidDataException("Api call failed", innerException: e);
            }
            return SessionHandler.Me;
        }

        public (string, string, string) GetTokenRenewStuffs()
        {
            var sql = "SELECT ident, pass, uid FROM Session";
            using (var cur = this.conn.CreateCommand())
            {
                cur.CommandText = sql;
                using (var row = cur.ExecuteReader())
                {
                    row.Read();

                    return (row.GetString(0), row.GetString(1), row.GetString(2));
                }
            }
        }

        public void SetMe(Me me, string uid, string pass, bool just_register = false)
        {
            var sql = "DELETE FROM Session";
            using (var cur = this.conn.CreateCommand())
            {
                cur.CommandText = sql;
                cur.ExecuteNonQuery();
            }

            sql = "INSERT OR REPLACE INTO Session(ident, firstName, lastName, showPwdChangeReminder, token, release, expire, uid, pass) VALUES($ident, $firstName, $lastName, $showPwdChangeReminder, $token, $release, $expire, $uid, $pass)";
            using (var cur = this.conn.CreateCommand())
            {
                cur.CommandText = sql;
                cur.Parameters.AddWithValue("$ident", me.Ident);
                cur.Parameters.AddWithValue("$firstName", me.FirstName);
                cur.Parameters.AddWithValue("$lastName", me.LastName);
                cur.Parameters.AddWithValue("$showPwdChangeReminder", me.ShowPwdChangeReminder);
                cur.Parameters.AddWithValue("$token", me.Token);
                cur.Parameters.AddWithValue("$release", me.Release);
                cur.Parameters.AddWithValue("$expire", me.Expire);
                cur.Parameters.AddWithValue("$uid", uid);
                cur.Parameters.AddWithValue("$pass", pass);
                cur.ExecuteNonQuery();
            }

            if (just_register is false)
                SessionHandler.Me = me;
        }

        public void RenewToken(DateTime? expire = null)
        {
            if ((expire ?? Me!.Expire) < DateTime.Now)
            {
                Client.INSTANCE.UnSetLoginToken();

                (var ident, var pass, var uid) = this.GetTokenRenewStuffs();
                var me = Client.INSTANCE.Login(ident: ident, pass: pass, uid: uid).ConfigureAwait(false).GetAwaiter().GetResult();
                // An ident maybe an email and have multiple accounts attached
                this.SetMe((Me)me, uid, pass);
            }
        }

        public int GetCacheSize()
        {
            var sql = "SELECT SUM((SELECT (length(url_path) + length(response) + length(update_date) + length(etag)) FROM RequestsCache))";
            using (var cur = this.conn.CreateCommand())
            {
                cur.CommandText = sql;
                using (var row = cur.ExecuteReader())
                {
                    row.Read();

                    return row.IsDBNull(0) ? 0 : row.GetInt32(0);
                }
            }
        }

        public void DropCache()
        {
            var sql = "DELETE FROM RequestsCache";
            using (var cur = this.conn.CreateCommand())
            {
                cur.CommandText = sql;
                cur.ExecuteNonQuery();
            }
        }

        public void SetNotificationsFlag(bool isChecked)
        {
            var sql = "UPDATE Settings SET NotificationsEnabled = $NE";
            using (var cur = this.conn.CreateCommand())
            {
                cur.CommandText = sql;
                cur.Parameters.AddWithValue("$NE", isChecked);
                cur.ExecuteNonQuery();
            }

            if (NotificationsFlagChanged is not null)
                NotificationsFlagChanged(this, isChecked);
        }

        public bool GetNotificationsFlag()
        {
            var sql = "SELECT NotificationsEnabled FROM Settings";
            using (var cur = this.conn.CreateCommand())
            {
                cur.CommandText = sql;
                using (var row = cur.ExecuteReader())
                {
                    row.Read();
                    return row.GetBoolean(0);
                }
            }
        }

        public void SetNotificationsRange(int range)
        {
            var sql = "UPDATE Settings SET NotificationsRange = $R";
            using (var cur = this.conn.CreateCommand())
            {
                cur.CommandText = sql;
                cur.Parameters.AddWithValue("$R", range);
                cur.ExecuteNonQuery();
            }

            if (NotificationsRangeChanged is not null)
                NotificationsRangeChanged(this, range);
        }

        public int GetNotificationsRange()
        {
            var sql = "SELECT NotificationsRange FROM Settings";
            using (var cur = this.conn.CreateCommand())
            {
                cur.CommandText = sql;
                using (var row = cur.ExecuteReader())
                {
                    row.Read();
                    return row.GetInt32(0);
                }
            }
        }

        public int GetPagesStackSize()
        {
            var sql = "SELECT PagesStackSize FROM Settings";
            using (var cur = this.conn.CreateCommand())
            {
                cur.CommandText = sql;
                using (var row = cur.ExecuteReader())
                {
                    row.Read();
                    return row.GetInt32(0);
                }
            }
        }

        public void SetPagesStackSize(int newValue)
        {
            var sql = "UPDATE Settings SET PagesStackSize = $PSZ";
            using (var cur = this.conn.CreateCommand())
            {
                cur.CommandText = sql;
                cur.Parameters.AddWithValue("$PSZ", newValue);
                cur.ExecuteNonQuery();
            }

            if (PagesStackSizeChanged is not null)
                PagesStackSizeChanged(this, newValue);
        }

        public int? GetYear()
        {
            var sql = "SELECT Year FROM Settings";
            using (var cur = this.conn.CreateCommand())
            {
                cur.CommandText = sql;
                using (var row = cur.ExecuteReader())
                {
                    row.Read();
                    return row.IsDBNull(0) ? null :  row.GetInt32(0);
                }
            }
        }

        public void SetYear(int newValue)
        {
            var sql = "UPDATE Settings SET Year = $y";
            using (var cur = this.conn.CreateCommand())
            {
                cur.CommandText = sql;
                cur.Parameters.AddWithValue("$y", newValue);
                cur.ExecuteNonQuery();
            }

            if (PagesStackSizeChanged is not null)
                PagesStackSizeChanged(this, newValue);
        }


        public void Close()
        {
            SqliteConnection.ClearPool(this.conn);
            this.conn.Handle!.manual_close();
            this.conn.Handle!.manual_close_v2();
            this.conn.Handle!.Close();
            this.conn.Close();
            this.conn.Dispose();
            this.conn = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public static void DestroyFile(string v)
        {
            for (int i = 0; i < 2; i++)
            {
                try
                {
                    File.Delete(v);
                }
                catch (Exception e)
                {
                    ;
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }

        }
    }
}
