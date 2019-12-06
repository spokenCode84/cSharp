using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Plch.OsTicket.Models.services
{
    public class Topics
    {
        [JsonProperty("instance", NullValueHandling = NullValueHandling.Ignore)]
        public string Instance { get; set; }

        [JsonProperty("Topics")]
        public List<Topic> HelpTopics { get; set; }
    }
}

