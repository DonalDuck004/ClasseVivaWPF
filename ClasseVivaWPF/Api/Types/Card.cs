using Newtonsoft.Json;
using System;


namespace ClasseVivaWPF.Api.Types
{
    public class Card : ApiObject
    {
        [JsonProperty(Required = Required.Always)]
        public required string Ident { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string UsrType { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string UsrId { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string MiurSchoolCode { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string MiurDivisionCode { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string FirstName { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string LastName { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required DateTime BirthDate { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string FiscalCode { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string SchCode { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string SchName { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string SchDedication { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string SchCity { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string SchProv { get; init; }

        public string FullName
        {
            get
            {
                var rt = this.UsrType == "G" ? "Genitore di " : "";
                return rt + this.FirstName + " " + this.LastName;
            }
        }

        public string Initials => $"{this.FirstName[0]}{this.LastName[0]}";
    }
}
