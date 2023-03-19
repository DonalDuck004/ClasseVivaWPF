using System;
using Newtonsoft.Json;

namespace ClasseVivaWPF.Api.Types
{
    public record class Content([JsonProperty("id_contenuto")] int ContentID,
                                string App,
                                [JsonProperty("ordine")] int Order,
                                object[] Tags,
                                [JsonProperty("inizio")] DateTime BeginDate,
                                [JsonProperty("fine")] DateTime EndDate,
                                [JsonProperty("scadenza")] DateTime? ExpireDate,
                                [JsonProperty("Tipo")] string Type,
                                [JsonProperty("titolo")] string Title,
                                string? Link,
                                string MediaType,
                                [JsonProperty("PanoramicaImg")] string PanoramicImg,
                                string? PanoramicaPos,
                                string Gallery,
                                ContentDetail[] ContentDetail,
                                RelatedContentDetail[] Related,
                                bool OpensExternally,
                                string AccessibilityLabel) : ApiObject
    {
        public const string TYPE_HOME_TOP = "Home top";
        public const string TYPE_MINIGAMES = "Minigames";
        public const string TYPE_PILLOLE = "Pillole";
        public const string TYPE_POPFESSORI = "Popfessori";

        public const string PANORAMIC_TOP = "top";
        public const string PANORAMIC_BANNER = "banner";

        public const string MEDIA_TYPE_VIDEO = "video";
        public const string MEDIA_TYPE_WEB = "web";
    }

}
