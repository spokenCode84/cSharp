using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Plch.OsTicket.Models.services
{
   public class TopicForm
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("dueDateField")]
        public string DueDateField { get; set; }
    }
}
