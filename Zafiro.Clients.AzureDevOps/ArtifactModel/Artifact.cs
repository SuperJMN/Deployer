using Newtonsoft.Json;

namespace Zafiro.Tools.AzureDevOps.ArtifactModel
{
    public class Artifact
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("resource")]
        public Resource Resource { get; set; }
    }
}