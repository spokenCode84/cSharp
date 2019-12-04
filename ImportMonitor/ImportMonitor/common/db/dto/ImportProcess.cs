using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace ImportMonitor.Common.Data
{
    class ImportProcess
    {
        // PROCESSCONFIGURATION
        [JsonProperty("Enabled")]
        public String Enabled { get; set; }

        [JsonProperty("ProcessDisplayName")]
        public String ProcessDisplayName { get; set; }

        [JsonProperty("ProcessDescription")]
        public String ProcessDescription { get; set; }


        // FSCONNECTIONCONFIGURATION
        [JsonProperty("ProcessingDirName")]
        public String ProcessingDirName { get; set; }

        [JsonProperty("SearchPattern")]
        public String SearchPattern { get; set; }

        [JsonProperty("DirProcessed")]
        public String DirProcessed { get; set; }

        [JsonProperty("DirImport")]
        public String DirImport { get; set; }

        [JsonProperty("DirError")]
        public String DirError { get; set; }

        // FSINTERACTIONCONFIGURATION
        [JsonProperty("Type")]
        public String Type { get; set; }

    }
}
