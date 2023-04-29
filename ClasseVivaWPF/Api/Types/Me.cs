using Newtonsoft.Json;
using System;
using System.Security.Policy;
using System.Text.RegularExpressions;


namespace ClasseVivaWPF.Api.Types
{
    public class Me : ApiObject
    {
        [JsonProperty(Required = Required.Always)]
        public required string Ident { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string FirstName { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string LastName { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required bool ShowPwdChangeReminder { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string Token { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required DateTime Release { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required DateTime Expire { get; init; }

        private int? _id = null;

        [JsonIgnore]
        public int Id => _id ??= int.Parse(new Regex("\\d+").Match(Ident).Value);

        [JsonIgnore]
        public char AccountType => this.Ident[0];

        public string FullName
        {
            get
            {
                var rt = this.AccountType == 'G' ? "Genitore di " : "";
                return rt + this.FirstName + " " + this.LastName;
            }
        }

        public string StudentFullName => this.FirstName + " " + this.LastName;
    };
}
