using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Plch.Notifications.osTicket.models
{
    class Topic : Listing
    {
        [JsonProperty("form")]
        public HelpTopicForm Form { get; set; }
    }
}
