using Newtonsoft.Json;
using System.Net.Http;

namespace ClasseVivaWPF.Api.Types
{
    public class S3FileHeader : ApiObject
    {
        [JsonProperty("id_file", Required = Required.Always)]
        public required int FileID;

        [JsonProperty(Required = Required.Always)]
        public required string Action;

        [JsonProperty(Required = Required.Always)]
        public required string Method;

        [JsonProperty(Required = Required.Always)]
        public required string EncType;

        [JsonProperty(Required = Required.Always)]
        public required string ACL;

        [JsonProperty(Required = Required.Always)]
        public required string Key;

        [JsonProperty("cont_disp", Required = Required.Always)]
        public required string ContentDisposition;

        [JsonProperty(Required = Required.Always)]
        public required int SAS;

        [JsonProperty("cont_type", Required = Required.Always)]
        public required string ContentType;

        [JsonProperty(Required = Required.Always)]
        public required string Tags;

        [JsonProperty(Required = Required.Always)]
        public required string XAC;

        [JsonProperty(Required = Required.Always)]
        public required string XAA;

        [JsonProperty(Required = Required.Always)]
        public required string XAD;

        [JsonProperty(Required = Required.Always)]
        public required string Policy;

        [JsonProperty(Required = Required.Always)]
        public required string XAS;

        [JsonProperty(Required = Required.Always)]
        public required string Region;

        [JsonProperty(Required = Required.Always)]
        public required string Bucket;


        [JsonIgnore]
        public HttpMethod EffectiveMethod => Method == "POST" ? HttpMethod.Post : HttpMethod.Get;
    }
}
