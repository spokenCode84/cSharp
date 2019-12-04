using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace ImportMonitor.Common.Data
{
    class UtilServer
    {
        [JsonProperty("ServerName")]
        public String ServerName { get; set; }

        [JsonProperty("Processes")]
        public List<ImportProcess> Processes { get; set; }

        public UtilServer() { }

        public UtilServer(String name) 
        {
            this.ServerName = name;
            this.Processes = new List<ImportProcess>();
        }
    }
}
