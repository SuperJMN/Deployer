﻿using Newtonsoft.Json;

namespace Zafiro.Tools.AzureDevOps.BuildsModel
{
    public class Timeline
    {

        [JsonProperty("href")]
        public string Href { get; set; }
    }
}