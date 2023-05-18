using ClasseVivaWPF.Api.Types;
using ClasseVivaWPF.Utils;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ClasseVivaWPF.Sessions
{
    public static class SessionMetaController
    {
        public static AccountMetaContainer Current { get; private set; }
        public const string FILENAME = "meta.json";
        public static string EFFECTIVE_PATH { get; private set; }

        static SessionMetaController()
        {
            SessionMetaController.EFFECTIVE_PATH = Path.Join(Config.SESSIONS_DIR_PATH, FILENAME);
            SessionMetaController.Current = SessionMetaController.Reload();
        }

        private static AccountMetaContainer Reload()
        {
            string content;
            if (!File.Exists(SessionMetaController.EFFECTIVE_PATH))
                content = "";
            else
                content = File.ReadAllText(SessionMetaController.EFFECTIVE_PATH);
            
            var obj = JsonConvert.DeserializeObject<AccountMetaContainer>(content) ?? new() { LastIdx = null, Accounts = new() };
            if (obj.LastIdx is null && obj.Accounts.Count != 0)
                obj.LastIdx = 0;

            return obj;
        }
       
        private static void Dump()
        {
            var content = JsonConvert.SerializeObject(SessionMetaController.Current);
            File.WriteAllText(SessionMetaController.EFFECTIVE_PATH, content);
        }

        public static bool AddAccount(Card card, bool SetAsCurrent = false) => AddAccount(new AccountMeta() { Ident = card.Ident, 
                                                                                                              Name = card.FullName,
                                                                                                              School = card.SchCode,
                                                                                                              Initials = card.Initials},
                                                                                           SetAsCurrent);

        public static bool AddAccount(AccountMeta meta, bool SetAsCurrent = false)
        {
            if (SessionMetaController.Current.Accounts.Where(x => x.Ident == meta.Ident).Any())
                return false;

          
            if (SessionMetaController.Current.LastIdx is null || SetAsCurrent)
                SessionMetaController.Current.LastIdx = SessionMetaController.Current.Accounts.Count;
            
            SessionMetaController.Current.Accounts.Add(meta);
            SessionMetaController.Dump();

            return true;
        }
        
        public static void Select(AccountMeta meta)
        {
            SessionMetaController.Current.LastIdx = SessionMetaController.Current.Accounts.TakeWhile(x => x.Ident != meta.Ident).Count();
            SessionMetaController.Dump();
        }

        private static void RemoveAt(int idx, bool dump = true) // Private as unsafe index
        {
            SessionMetaController.Current.Accounts.RemoveAt(idx);

            if (dump)
                SessionMetaController.Dump();
        }

        private static void SelectAt(int idx)
        {
            SessionMetaController.Current.LastIdx = idx;
            SessionMetaController.Dump();
        }

        public static void RemoveCurrent(int new_idx = 0)
        {
            if (new_idx > SessionMetaController.Current.LastIdx)
                new_idx--;

            SessionMetaController.Current.Accounts.RemoveAt(SessionMetaController.Current.LastIdx!.Value);
            SessionMetaController.Current.LastIdx = new_idx;

            SessionMetaController.Dump();
        }

        public static void Remove(AccountMeta meta)
        {
            var idx = SessionMetaController.Current.Accounts.TakeWhile(x => x.Ident != meta.Ident).Count();
            RemoveAt(idx, dump: false);

            if (idx == SessionMetaController.Current.LastIdx)
                SessionMetaController.Current.LastIdx = SessionMetaController.Current.Accounts.Count == 0 ? null : 0;
            else if (idx < SessionMetaController.Current.LastIdx)
                SessionMetaController.Current.LastIdx--;

            SessionMetaController.Dump();
        }
    }
}
