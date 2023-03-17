using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using ClasseVivaWPF.Api.Types;
using Newtonsoft.Json.Linq;
using System.Windows;
using System.Runtime.ConstrainedExecution;

namespace ClasseVivaWPF.Sessions
{
    public class SessionHandler
    {
        public static SessionHandler? INSTANCE { get; private set; }

        private static Me? _me = null;
        public static Me? Me { get => _me;
            private set {
                _me = value;
                if (value is not null)
                    Api.Client.INSTANCE!.SetLoginToken(value.Token);
            } 
        }

        private SqliteConnection conn;

        private SessionHandler(string session_name) {
            session_name += ".db";

            var exists = File.Exists(session_name);
            this.conn = new SqliteConnection($"Data Source={session_name}");
            this.conn.Open();
            if (!exists)
                this.LoadSchema();
        }

        public static bool TryInit()
        {
            if (!Directory.Exists("Sessions"))
                Directory.CreateDirectory("Sessions");

            if (File.Exists("Sessions/Last"))
            {
                using (var sr = new StreamReader("Sessions/Last"))
                {
                    var l = sr.ReadLine();
                    if (l is null)
                        return false;

                    var @this = new SessionHandler($"Sessions/{l}");
                    try
                    {
                        @this.GetMe();
                    }catch(InvalidDataException)
                    {
                        @this.Destroy();
                        return false;
                    }
                    INSTANCE = @this;
                    return true;
                }
            }

            return false;
        }

        public static SessionHandler InitConn(int user_id)
        {
            return INSTANCE = new($"Sessions/{user_id}");
        }

        public void LoadSchema()
        {
            var sql = "CREATE TABLE Session(ident VARCHAR(16) PRIMARY KEY, firstName VARCHAR(48) NOT NULL, lastName VARCHAR(48) NOT NULL, showPwdChangeReminder BOOL NOT NULL, token VARCHAR(512) NOT NULL, release VARCHAR(25) NOT NULL, expire VARCHAR(25) NOT NULL)";
            var cur = this.conn.CreateCommand();
            cur.CommandText = sql;
            cur.ExecuteNonQuery();


            sql = @"CREATE TABLE RequestCache(url_path VARCHAR(256) PRIMARY KEY, response TEXT, update_date VARCHAR(25) NOT NULL)";
            cur = this.conn.CreateCommand();
            cur.CommandText = sql;
            cur.ExecuteNonQuery();
        }

        public DateTime? CheckCache(string url_path, out string? response)
        {
            var sql = "SELECT response, update_date FROM RequestCache WHERE url_path = $url_path";
            var cur = this.conn.CreateCommand();
            cur.CommandText = sql;
            cur.Parameters.AddWithValue("$url_path", url_path);
            var r = cur.ExecuteReader();
            if (r.Read())
            {
                response = r.GetString(0);
                return r.GetDateTime(1);
            }

            response = null;
            return null;
        }

        public void SetCache(string url_path, string response)
        {
            var sql = "INSERT OR REPLACE INTO RequestCache(url_path, response, update_date) VALUES ($url_path, $response, DATE('now'))";
            var cur = this.conn.CreateCommand();
            cur.CommandText = sql;
            cur.Parameters.AddWithValue("$url_path", url_path);
            cur.Parameters.AddWithValue("$response", response);
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

            SessionHandler.Me = new(Ident: result.GetString(0),
                                    FirstName: result.GetString(1),
                                    LastName: result.GetString(2),
                                    ShowPwdChangeReminder: result.GetBoolean(3),
                                    Token: result.GetString(4),
                                    Release: result.GetDateTime(5),
                                    Expire: result.GetDateTime(6));

            if (SessionHandler.Me.Expire <= DateTime.Now)
                throw new InvalidDataException("Session expired");

            return SessionHandler.Me;
        }

        public void SetMe(Me me)
        {
            var sql = "DELETE FROM Session";
            var cur = this.conn.CreateCommand();
            cur.CommandText = sql;
            cur.ExecuteNonQuery();
          
            sql = "INSERT INTO Session(ident, firstName, lastName, showPwdChangeReminder, token, release, expire) VALUES($ident, $firstName, $lastName, $showPwdChangeReminder, $token, $release, $expire)";
            cur = this.conn.CreateCommand();
            cur.CommandText = sql;
            cur.Parameters.AddWithValue("$ident", me.Ident);
            cur.Parameters.AddWithValue("$firstName", me.FirstName);
            cur.Parameters.AddWithValue("$lastName", me.LastName);
            cur.Parameters.AddWithValue("$showPwdChangeReminder", me.ShowPwdChangeReminder);
            cur.Parameters.AddWithValue("$token", me.Token);
            cur.Parameters.AddWithValue("$release", me.Release);
            cur.Parameters.AddWithValue("$expire", me.Expire);
            cur.ExecuteNonQuery();

            SessionHandler.Me = me;

            using (var sw = new StreamWriter("Sessions/Last", false))
            {
                sw.WriteLine(me.Id.ToString());
            }
        }
    }
}
