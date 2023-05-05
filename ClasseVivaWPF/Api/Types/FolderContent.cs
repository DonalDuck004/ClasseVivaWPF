using ClasseVivaWPF.Utils;
using ClasseVivaWPF.Utils.Interfaces;
using Microsoft.Toolkit.Uwp.Notifications;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace ClasseVivaWPF.Api.Types
{
    public class FolderContent : ApiObject, IBuildNotifyDidatic
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

        [JsonIgnore]
        public int EffectiveID => this.GetHashCode();

        public void BuildNotify(ToastContentBuilder toast)
        {
            toast.AddText("Nuovo materiale");
            toast.AddText(this.ContentName);
        }

        public async Task<FolderContentContentItem> GetItem()
        {
            if (this._CachedItem is null)
                this._CachedItem = await Client.INSTANCE.GetDidaticitem(this.ContentID);

            return this._CachedItem;
        }

        public string GetSection() => NotificationSystem.GOTO_DIDATIC;
        public int GetHighlightID() => this.ContentID;

        public override int GetHashCode()
        {
            return HashCode.Combine(this.GetType().Name, this.ContentID);
        }
    }
}
