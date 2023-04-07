﻿using ClasseVivaWPF.Api;
using ClasseVivaWPF.Api.Types;
using ClasseVivaWPF.Utils;
using Microsoft.Data.Sqlite;
using System;
using System.IO;

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

        public static bool TryInit(out string? api_error_message)
        {
            api_error_message = null;
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
                catch (InvalidDataException e)
                {
                    if (e.InnerException is ApiError ae)
                        api_error_message = ae.Message;

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

            sql = "CREATE TABLE Settings(NotificationsEnabled BOOL DEFAULT True, NotificationsRange INT DEFAULT 6, PagesStackSize INT DEFAULT 5)";
            cur = this.conn.CreateCommand();
            cur.CommandText = sql;
            cur.ExecuteNonQuery();

            sql = "INSERT INTO Settings DEFAULT VALUES";
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
            
            return row.IsDBNull(0) ? 0 : row.GetInt32(0);
        }

        public void DropCache()
        {
            var sql = "DELETE FROM RequestsCache";
            var cur = this.conn.CreateCommand();
            cur.CommandText = sql;
            cur.ExecuteNonQuery();
        }

        public void SetNotificationsFlag(bool isChecked)
        {
            var sql = "UPDATE Settings SET NotificationsEnabled = $NE";
            var cur = this.conn.CreateCommand();
            cur.CommandText = sql;
            cur.Parameters.AddWithValue("$NE", isChecked);
            cur.ExecuteNonQuery();

            if (NotificationsFlagChanged is not null)
                NotificationsFlagChanged(this, isChecked);
        }

        public bool GetNotificationsFlag()
        {
            var sql = "SELECT NotificationsEnabled FROM Settings";
            var cur = this.conn.CreateCommand();
            cur.CommandText = sql;
            var result = cur.ExecuteReader();
            result.Read();
            return result.GetBoolean(0);
        }

        public void SetNotificationsRange(int range)
        {
            var sql = "UPDATE Settings SET NotificationsRange = $R";
            var cur = this.conn.CreateCommand();
            cur.CommandText = sql;
            cur.Parameters.AddWithValue("$R", range);
            cur.ExecuteNonQuery();

            if (NotificationsRangeChanged is not null)
                NotificationsRangeChanged(this, range);
        }

        public int GetNotificationsRange()
        {
            var sql = "SELECT NotificationsRange FROM Settings";
            var cur = this.conn.CreateCommand();
            cur.CommandText = sql;
            var result = cur.ExecuteReader();
            result.Read();
            var x = result.GetInt32(0);
            return x;
        }

        public int GetPagesStackSize()
        {
            var sql = "SELECT PagesStackSize FROM Settings";
            var cur = this.conn.CreateCommand();
            cur.CommandText = sql;
            var result = cur.ExecuteReader();
            result.Read();
            return result.GetInt32(0);
        }

        public void SetPagesStackSize(int newValue)
        {
            var sql = "UPDATE Settings SET PagesStackSize = $PSZ";
            var cur = this.conn.CreateCommand();
            cur.CommandText = sql;
            cur.Parameters.AddWithValue("$PSZ", newValue);
            cur.ExecuteNonQuery();

            if (PagesStackSizeChanged is not null)
                PagesStackSizeChanged(this, newValue);
        }
    }
}
