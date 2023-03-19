using System;
using ClasseVivaWPF.Api.Types;
using Newtonsoft.Json;

namespace ClasseVivaWPF.Api.Types
{
    public class Content : ApiObject
    {
        [JsonProperty("id_contenuto", Required = Required.Always)]
        public required int ContentID {get; init;}

        [JsonProperty(Required = Required.Always)]
        public required string App {get; init;}

        [JsonProperty("ordine", Required = Required.Always)]
        public required int Order {get; init;}

        [JsonProperty(Required = Required.AllowNull)]
        public required object[]? Tags {get; init;}

        [JsonProperty("inizio", Required = Required.Always)]
        public required DateTime BeginDate {get; init;}

        [JsonProperty("fine", Required = Required.Always)]
        public required DateTime EndDate {get; init;}

        [JsonProperty("scadenza", Required = Required.AllowNull)]
        public required DateTime? ExpireDate {get; init;}

        [JsonProperty("tipo", Required = Required.Always)]
        public required string Type {get; init;}

        [JsonProperty("titolo", Required = Required.Always)]
        public required string Title {get; init;}

        [JsonProperty(Required = Required.AllowNull)]
        public required string? Link {get; init;}

        [JsonProperty("media_type", Required = Required.AllowNull)]
        public required string? MediaType {get; init;}

        [JsonProperty("PanoramicaImg", Required = Required.AllowNull)]
        public required string? PanoramicImg {get; init;}

        [JsonProperty(Required = Required.AllowNull)]
        public required string? PanoramicaPos {get; init;}

        [JsonProperty(Required = Required.AllowNull)] 
        public required string? Gallery {get; init;}

        [JsonProperty("content_detail", Required = Required.AllowNull)]
        public required ContentDetail[]? ContentDetail {get; init;}

        [JsonProperty(Required = Required.Always)]
        public required RelatedContentDetail[] Related {get; init;}

        [JsonProperty("opens_externally", Required = Required.Always)]
        public required bool OpensExternally {get; init;}

        [JsonProperty("accessibility_label", Required = Required.AllowNull)]
        public required string AccessibilityLabel { get; init; }


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
