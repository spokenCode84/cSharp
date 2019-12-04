using System;
using System.IO;
using System.Collections.Generic;

using Newtonsoft.Json;


namespace ImportMonitor.Common
{
    internal class Env
    {
        public static String BASE_DIR = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "..", "..");
        public static String CFG_DIR = BASE_DIR + @"\config";
        public static String CACHE_DIR = BASE_DIR + @"\cache";

        [JsonProperty("ResetCache")]
        public static Boolean ResetCache { get; set; }

        [JsonProperty("DEBUG")]
        public static Boolean DEBUG { get; set; }

        [JsonProperty("DataSource")]
        public static Dictionary<String, String> DataSource { get; set; }

        [JsonProperty("AppServer")]
        public static Dictionary<String, String> AppServer { get; set; }

        [JsonProperty("UtilServers")]
        public static List<Dictionary<String, String>> UtilServers { get; set; }

        [JsonProperty("RunJobs")]
        public static Dictionary<String, Boolean> RunJobs { get; set; }

        [JsonProperty("DaysToKeep")]
        public static int DaysToKeep { get; set; }

    }
}