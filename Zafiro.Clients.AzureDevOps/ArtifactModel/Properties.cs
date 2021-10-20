using Newtonsoft.Json;

namespace Zafiro.Tools.AzureDevOps.ArtifactModel
{
    public class Properties
    {

        [JsonProperty("localpath")]
        public string Localpath { get; set; }
    }
}
