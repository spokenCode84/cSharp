using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CRM.Helper.Web_API_Helper_Code.Logging.Models
{
	public class FilteredRequest
	{
		[JsonProperty("selectedItems", NullValueHandling = NullValueHandling.Ignore)]
		public SelectedItems selectedItems { get; set; }

		[JsonProperty("requestContains", NullValueHandling = NullValueHandling.Ignore)]
		public string requestContains { get; set; }

		[JsonProperty("responseContains", NullValueHandling = NullValueHandling.Ignore)]
		public string responseContains { get; set; }

		[JsonProperty("dateFrom", NullValueHandling = NullValueHandling.Ignore)]
		public string dateFrom { get; set; }

		[JsonProperty("dateTo", NullValueHandling = NullValueHandling.Ignore)]
		public string dateTo { get; set; }

		[JsonProperty("timeFrom", NullValueHandling = NullValueHandling.Ignore)]
		public Dictionary<string, string> timeFrom { get; set; }

		[JsonProperty("timeTo", NullValueHandling = NullValueHandling.Ignore)]
		public Dictionary<string, string> timeTo { get; set; }


	}
}
