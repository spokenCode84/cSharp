using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Plch.OsTicket.Models.services
{
   public class Topic : ServiceType
    {
        [JsonProperty("form")]
        public TopicForm Form { get; set; }
    }
}
