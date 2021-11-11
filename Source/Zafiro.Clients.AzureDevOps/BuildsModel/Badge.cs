using Newtonsoft.Json;

namespace Zafiro.Tools.AzureDevOps.BuildsModel
{
    public class Badge
    {

        [JsonProperty("href")]
        public string Href { get; set; }
    }
}