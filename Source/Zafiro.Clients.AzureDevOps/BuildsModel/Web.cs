﻿using Newtonsoft.Json;

namespace Zafiro.Tools.AzureDevOps.BuildsModel
{
    public class Web
    {

        [JsonProperty("href")]
        public string Href { get; set; }
    }
}