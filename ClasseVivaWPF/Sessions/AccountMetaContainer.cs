using Newtonsoft.Json;
using System.Collections.Generic;

namespace ClasseVivaWPF.Sessions
{
    public class AccountMetaContainer
    {
        [JsonProperty(Required = Required.Always)]
        public required int? LastIdx { get; set; }

        [JsonProperty(Required = Required.Always)]
        public required List<AccountMeta> Accounts { get; set; }

        [JsonIgnore()]
        public bool HasAccounts => this.Accounts.Count != 0;

        [JsonIgnore()]
        public AccountMeta? CurrentAccount => this.LastIdx is null ? null : this.Accounts[this.LastIdx.Value];
    }
}
