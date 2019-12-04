using System;
using System.Net;
using System.Web.Http;
using System.Net.Http;
using Newtonsoft.Json;
using CRM.Helper.Web_API_Helper_Code.Logging;
using CRM.Helper.Web_API_Helper_Code.Logging.Models;
using CRM.Helper.Web_API_Helper_Code.Logging.Exceptions;
using CRM.Helper.Web_API_Helper_Code.Logging.QueryBuilder;
using System.Collections.Generic;

namespace WebAPI.Controllers
{
	public class LoggingController : ApiController
	{
		private CrmLogger log;

		public LoggingController()
		{
			log = new CrmLogger();
		}
		// GET api/<controller>
		public IHttpActionResult Get()
		{
			return BadRequest("THis is a get request");
		}


		// GET api/<controller>/5
		public string Get(string column)
		{
			QueryBuilder qb = new QueryBuilder();
			List<string> values = qb.GetDistinctColumnValues(column);
			return string.Join<string>(",", values);
			// Display.
		}

		// POST api/<controller>
		[HttpPost]
		public IHttpActionResult Post(HttpRequestMessage request)
		{
			LogRequest logRequest = JsonConvert.DeserializeObject<LogRequest>(Request.Content.ReadAsStringAsync().Result);

			try
			{
				if (string.IsNullOrEmpty(logRequest.message))
					throw new RequiredParameterException($"Invalid request.  Provide message parameter");

				if (string.IsNullOrEmpty(logRequest.type))
					throw new RequiredParameterException($"Invalid request.  Provide provide type parameter");

				switch (logRequest.type.ToLower())
				{
					case "info":
						log.Info(logRequest);
						break;
					case "debug":
						log.Debug(logRequest);
						break;
					case "error":
						log.Error(logRequest);
						break;
					default:
						throw new InvalidParameterException($"Invalid Logging Type: {logRequest.type}");
				}
			}
			catch (LoggingException e)
			{
				return BadRequest(e.Message);
			}
			return Ok("Record Created");
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