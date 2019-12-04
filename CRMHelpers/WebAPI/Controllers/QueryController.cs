using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using CRM.Helper.Web_API_Helper_Code.Logging;
using CRM.Helper.Web_API_Helper_Code.Logging.Models;
using CRM.Helper.Web_API_Helper_Code.Logging.Exceptions;
using CRM.Helper.Web_API_Helper_Code.Logging.QueryBuilder;


namespace WebAPI.Controllers
{
	public class QueryController : ApiController
	{

		private Version webAPIVersion = new Version(8, 0);

		private string getVersionedWebAPIPath()
		{
			return string.Format("v{0}/", webAPIVersion.ToString(2));
		}


		// GET api/<controller>
		public IEnumerable<string> Get()
		{
			HttpRequestMessage RetrieveVersionRequest =
			new HttpRequestMessage(HttpMethod.Get, getVersionedWebAPIPath() + "RetrieveVersion");
			return new string[] { getVersionedWebAPIPath() + "RetrieveVersion", "value2" };
		}

		// GET api/<controller>/5
		public string Get(int id)
		{
			return "value";
		}

		// POST api/<controller>
		// POST api/<controller>
		[HttpPost]
		public IHttpActionResult Post(HttpRequestMessage httpRequest)
		{

			try
			{
				QueryBuilder qb = new QueryBuilder(QueryBuilder.formatTimestamps(
					JsonConvert.DeserializeObject<FilteredRequest>(Request.Content.ReadAsStringAsync().Result)));
				return Ok(qb.BuildQuery().GetFilteredResultSet());
			}
			catch(Exception e)
			{
				return BadRequest(e.Message);
			}

		}

		// PUT api/<controller>/5
		public void Put(int id, [FromBody]string value)
		{
		}

		// DELETE api/<controller>/5
		public void Delete(int id)
		{
		}
	}
}