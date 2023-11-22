using Newtonsoft.Json;

namespace Zafiro.Tools.AzureDevOps.BuildsModel
{
    public class Plan
    {

        [JsonProperty("planId")]
        public string PlanId { get; set; }
    }
}