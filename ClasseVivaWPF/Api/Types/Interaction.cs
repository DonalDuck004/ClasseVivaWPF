using Newtonsoft.Json;
using System.Diagnostics;
using System.Linq;

namespace ClasseVivaWPF.Api.Types
{
    public class Interaction : ApiObject
    {
        [JsonProperty("id_contenuto", Required = Required.Always)]
        public required int ContentID { get; init; }

        [JsonProperty("id_utente", Required = Required.Always)]
        public required int UserID { get; init; }

        [JsonProperty("tipo_interazione", Required = Required.Always)]
        public required string[] InteractionTypes { get; init; }

        [JsonProperty("piace_a", Required = Required.Always)]
        public required int LikesTo { get; init; }


        public const string REACTION_LIKE = "like";
        public const string REACTION_BOOKMARK = "bookmark";

        public bool IsLiked => InteractionTypes.Contains(REACTION_LIKE);

        public bool IsSaved => InteractionTypes.Contains(REACTION_BOOKMARK);
    }
}
