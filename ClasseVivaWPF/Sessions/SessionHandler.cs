using ClasseVivaWPF.Api;
using ClasseVivaWPF.Api.Types;
using ClasseVivaWPF.Utils;
using Microsoft.Data.Sqlite;
using System;
using System.IO;

namespace ClasseVivaWPF.Sessions
{
    public class SessionHandler
    {
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
        private SqliteConnection conn { get; init; }
        
        private SessionHandler(string session_name)
        {
            this.FileName = session_name + ".db";

            var exists = File.Exists(this.FileName);
            this.conn = new SqliteConnection($"Data Source={this.FileName}");
            this.conn.Open();
            if (!exists)
                this.LoadSchema();
        }

        public static bool TryInit()
        {
            var last = Path.Join(Config.SESSIONS_DIR_PATH, "Last");

            if (File.Exists(last))
            {
                string? l;
                using (var sr = new StreamReader(last))
                {
                    l = sr.ReadLine();
                    if (l is null)
                        return false;
                }

                var @this = new SessionHandler(Path.Join(Config.SESSIONS_DIR_PATH, l));
                try
                {
                    @this.GetMe();
                }
                catch (InvalidDataException)
                {
                    @this.Destroy();
                    return false;
                }
                INSTANCE = @this;
                return true;
            }

            return false;
        }

        public static SessionHandler InitConn(int user_id)
        {
            return INSTANCE = new(Path.Join(Config.SESSIONS_DIR_PATH, user_id.ToString()));
        }

        public void LoadSchema()
        {
            var sql = "CREATE TABLE Session(ident VARCHAR(16) PRIMARY KEY, firstName VARCHAR(48) NOT NULL, lastName VARCHAR(48) NOT NULL, showPwdChangeReminder BOOL NOT NULL, token VARCHAR(512) NOT NULL, release VARCHAR(25) NOT NULL, expire VARCHAR(25) NOT NULL, uid VARCHAR(128) NOT NULL, pass NOT NULL)";
            var cur = this.conn.CreateCommand();
            cur.CommandText = sql;
            cur.ExecuteNonQuery();


            sql = "CREATE TABLE RequestsCache(url_path VARCHAR(256) PRIMARY KEY, response TEXT, update_date VARCHAR(32) NOT NULL, etag VARCHAR(32))";
            cur = this.conn.CreateCommand();
            cur.CommandText = sql;
            cur.ExecuteNonQuery();
        }

        public DateTime? CheckCache(string url_path, out string? response, out string? etag)
        {
            var sql = "SELECT response, etag, update_date FROM RequestsCache WHERE url_path = $url_path";
            var cur = this.conn.CreateCommand();
            cur.CommandText = sql;
            cur.Parameters.AddWithValue("$url_path", url_path);
            var r = cur.ExecuteReader();
            if (r.Read())
            {
                response = r.GetString(0);
                etag = r.IsDBNull(1) ? null : r.GetString(1);
                return r.GetDateTime(2);
            }

            etag = response = null;
            return null;
        }

        public void SetCache(string url_path, string response, string? etag)
        {
            var sql = "INSERT OR REPLACE INTO RequestsCache(url_path, response, update_date, etag) VALUES ($url_path, $response, DATE('now'), $etag)";
            var cur = this.conn.CreateCommand();
            cur.CommandText = sql;
            cur.Parameters.AddWithValue("$url_path", url_path);
            cur.Parameters.AddWithValue("$response", response);
            cur.Parameters.AddWithValue("$etag", etag is null ? DBNull.Value : etag);
            cur.ExecuteNonQuery();
        }

        public void Destroy()
        {
            var sql = "DELETE FROM Session";
            var cur = this.conn.CreateCommand();
            cur.CommandText = sql;
            cur.ExecuteNonQuery();
        }

        public Me GetMe()
        {
            var sql = "SELECT ident, firstName, lastName, showPwdChangeReminder, token, release, expire FROM Session";
            var cur = this.conn.CreateCommand();
            cur.CommandText = sql;
            var result = cur.ExecuteReader();
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

            this.RenewToken(SessionHandler.Me.Expire);

            return SessionHandler.Me;
        }

        public (string, string, string) GetTokenRenewStuffs()
        {
            var sql = "SELECT ident, pass, uid FROM Session";
            var cur = this.conn.CreateCommand();
            cur.CommandText = sql;
            var row = cur.ExecuteReader();
            row.Read();

            return (row.GetString(0), row.GetString(1), row.GetString(2));
        }

        public void SetMe(Me me, string uid, string pass)
        {
            var sql = "DELETE FROM Session";
            var cur = this.conn.CreateCommand();
            cur.CommandText = sql;
            cur.ExecuteNonQuery();

            sql = "INSERT INTO Session(ident, firstName, lastName, showPwdChangeReminder, token, release, expire, uid, pass) VALUES($ident, $firstName, $lastName, $showPwdChangeReminder, $token, $release, $expire, $uid, $pass)";
            cur = this.conn.CreateCommand();
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

            SessionHandler.Me = me;

            var sw = new StreamWriter(Path.Join(Config.SESSIONS_DIR_PATH, "Last"), false);
            sw.WriteLine(me.Id.ToString());
            sw.Close();
        }

        public void RenewToken(DateTime? expire = null)
        {
            if ((expire ?? Me!.Expire) < DateTime.Now)
            {
                Client.INSTANCE.UnSetLoginToken();

                (var ident, var pass, var uid) = this.GetTokenRenewStuffs();
                var me = Client.INSTANCE.Login(ident: ident, pass: pass, uid: uid).ConfigureAwait(false).GetAwaiter().GetResult();
                this.SetMe(me, uid, pass);
            }
        }


        public int GetCacheSize()
        {
            var sql = "SELECT SUM((SELECT (length(url_path) + length(response) + length(update_date) + length(etag)) FROM RequestsCache))";
            var cur = this.conn.CreateCommand();
            cur.CommandText = sql;
            var row = cur.ExecuteReader();
            row.Read();
            return row.GetInt32(0);
        }
    }
}
