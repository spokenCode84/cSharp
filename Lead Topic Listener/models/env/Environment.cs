using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace IFB.CRM.Services.Lead.models.env
{
	class Env
	{
		public static String BaseDir = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "..", "..");
		public static String ConfigDir = $"{BaseDir}\\config";
		public static String EnvironmentKey = "CRM_ENV";
		public static String CurrentRegion = Environment.GetEnvironmentVariable(Env.EnvironmentKey).ToLower();

		[JsonProperty("activeMqCxn")]
		public static Dictionary<String, String> ActiveMq { get; set; }

		[JsonProperty("queues")]
		public static Dictionary<String, String> queues { get; set; }

	}
}