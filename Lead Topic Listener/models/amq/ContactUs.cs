using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace IFB.CRM.Services.Lead.models.amq
{
	class ContactUs
	{

		public string ExceedClientId { get; set; }
		public string ContactIdText { get; set; }

		[JsonProperty("source")]
		public string Source { get; set; }

		[JsonProperty("leadSourceCategory")]
		public string LeadSourceCategory { get; set; }

		[JsonProperty("firstName")]
		public string FirstName { get; set; }

		[JsonProperty("lastName")]
		public string LastName { get; set; }

		[JsonProperty("contactMethod")]
		public string ContactMethod { get; set; }

		[JsonProperty("phone")]
		public string Phone { get; set; }

		[JsonProperty("email")]
		public string Email { get; set; }

		[JsonProperty("formSourceSubCategory")]
		public string FormSourceSubCategory { get; set; }

		[JsonProperty("policyNumber")]
		public string PolicyNumber { get; set; }

		[JsonProperty("comments")]
		public string Comments { get; set; }

		[JsonProperty("additionalDetails")]
		public string AdditonalDetails { get; set; }

		[JsonProperty("policyType")]
		public string PolicyType { get; set; }

		[JsonProperty("agentFName")]
		public string AgentFName { get; set; }

		[JsonProperty("agentLName")]
		public string AgentLName { get; set; }

		[JsonProperty("agentEmail")]
		public string AgentEmail { get; set; }

		[JsonProperty("agentACID")]
		public string AgentACID { get; set; }

		[JsonProperty("street")]
		public string Street { get; set; }

		[JsonProperty("apt")]
		public string Apt { get; set; }

		[JsonProperty("city")]
		public string City { get; set; }

		[JsonProperty("state")]
		public string State { get; set; }

		[JsonProperty("zip")]
		public string Zip { get; set; }

		[JsonProperty("policyStreet")]
		public string PolicyStreet { get; set; }

		[JsonProperty("policyApt")]
		public string PolicyApt { get; set; }

		[JsonProperty("policyCity")]
		public string PolicyCity { get; set; }

		[JsonProperty("policyState")]
		public string PolicyState { get; set; }

		[JsonProperty("policyZip")]
		public string PolicyZip { get; set; }

		[JsonProperty("eventDate")]
		public string EventDate { get; set; }

		[JsonProperty("eventTime")]
		public string EventTime { get; set; }

		[JsonProperty("eventAmPm")]
		public string EventAmPm { get; set; }
	}
}
