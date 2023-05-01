using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace ClasseVivaWPF.Api.Types
{
    public class FolderContent : ApiObject
    {
        [JsonProperty(Required = Required.Always)]
        public required int ContentID { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string ContentName { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required int ObjectId { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required FolderContentType ObjectType { get; init;}

        [JsonProperty(Required = Required.Always)]
        public required DateTime ShareDT { get; init; }

        [JsonIgnore]
        private FolderContentContentItem? _CachedItem = null;

        [JsonIgnore]
        public FolderContentContentItem? CachedItem => _CachedItem;

        public async Task<FolderContentContentItem> GetItem()
        {
            if (this._CachedItem is null)
                this._CachedItem = await Client.INSTANCE.GetDidaticitem(this.ContentID);

            return this._CachedItem;
        }
    }
}
