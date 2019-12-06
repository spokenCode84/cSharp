using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Plch.OsTicket.Models.services
{
    public class Departments
    {
        [JsonProperty("instance", NullValueHandling = NullValueHandling.Ignore)]
        public string Instance { get; set; }

        [JsonProperty("departments")]
        public List<Department> departments { get; set; }
    }
}
