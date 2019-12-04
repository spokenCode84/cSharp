using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CRM.Helper.Web_API_Helper_Code.Logging.Models
{
	public class LogRequest
	{
		public string message { get; set; }

		public string type { get; set; }

		[JsonProperty("user", NullValueHandling = NullValueHandling.Ignore)]
		public string user { get; set; }

		[JsonProperty("source", NullValueHandling = NullValueHandling.Ignore)]
		public string source { get; set; }

		[JsonProperty("request", NullValueHandling = NullValueHandling.Ignore)]
		public string request { get; set; }

		[JsonProperty("response", NullValueHandling = NullValueHandling.Ignore)]
		public string response { get; set; }

		[JsonProperty("stacktrace", NullValueHandling = NullValueHandling.Ignore)]
		public string stacktrace { get; set; }

	}
}
