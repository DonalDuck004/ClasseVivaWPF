using Newtonsoft.Json;
using System.Diagnostics;
using System.Linq;

namespace ClasseVivaWPF.Api.Types
{
    public record class Interaction([JsonProperty("id_contenuto")] int ContentID,
                                    [JsonProperty("id_utente")] int UserID,
                                    [JsonProperty("tipo_interazione")] string[] InteractionTypes,
                                    [JsonProperty("piace_a")] int LikesTo) : ApiObject
    { 
        public const string REACTION_LIKE = "like";
        public const string REACTION_BOOKMARK = "bookmark";

        public bool IsLiked => InteractionTypes.Contains(REACTION_LIKE);

        public bool IsSaved => InteractionTypes.Contains(REACTION_BOOKMARK);
    }
}
