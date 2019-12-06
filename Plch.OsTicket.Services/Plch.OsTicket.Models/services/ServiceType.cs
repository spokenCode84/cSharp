using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Plch.OsTicket.Models.services
{
    public class ServiceType
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("pastDueDays")]
        public int PastDueDays { get; set; }

        [JsonProperty("recurring")]
        public Boolean Recurring { get; set; }

        [JsonProperty("additionalRecipients")]
        public List<string> AdditionalRecipients { get; set; }
    }
}
