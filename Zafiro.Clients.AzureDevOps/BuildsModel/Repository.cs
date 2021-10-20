﻿using Newtonsoft.Json;

namespace Zafiro.Tools.AzureDevOps.BuildsModel
{
    public class Repository
    {

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("clean")]
        public object Clean { get; set; }

        [JsonProperty("checkoutSubmodules")]
        public bool CheckoutSubmodules { get; set; }
    }
}