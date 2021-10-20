using Newtonsoft.Json;

namespace Zafiro.Tools.AzureDevOps.BuildsModel
{
    public class SourceVersionDisplayUri
    {

        [JsonProperty("href")]
        public string Href { get; set; }
    }
}