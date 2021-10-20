using Newtonsoft.Json;

namespace Zafiro.Tools.AzureDevOps.BuildsModel
{
    public class OrchestrationPlan
    {

        [JsonProperty("planId")]
        public string PlanId { get; set; }
    }
}