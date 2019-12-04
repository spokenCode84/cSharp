using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web.Http;
using CRM.Helper;
using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;
using WebAPI.CRMHelpers;

namespace WebAPI.Controllers
{
    // https://docs.microsoft.com/en-us/azure/architecture/patterns/cache-aside

    public class CRMCacheController : ApiController
    {
        //  ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");

        // GET: api/CRMCache
        // Example: http://localhost:62371/api/CRMCache?logicalName=ifb_externalsystemlinkses
        public HttpResponseMessage Get(string logicalName)
        {
            try
            {
                if (logicalName == String.Empty)
                    return null;

                // Define a unique key for this method and its parameters.
                var key = $"Entities:{logicalName}";

                Task<JObject> task = Task.Run(async () =>
                    await CRMRetrieval.GetAllRecordsForAnEntityAsync(key, logicalName));

                task.Wait();

                var result = task.Result;

                if (Request == null)
                {
                    HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);
                    message.Content = new ObjectContent(typeof(JObject), result, new JsonMediaTypeFormatter());
                    return message;
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
                }
            }
            catch (Exception ex)
            {
                if (Request == null)
                {
                    throw new HttpResponseException(HttpStatusCode.InternalServerError);

                    //HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                    //message.Content = new ObjectContent(typeof(Exception), ex, new JsonMediaTypeFormatter());
                    //return message;
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message + " " + ex.InnerException, ex);
                }
            }
        }

        public HttpResponseMessage GetEntityByID(string logicalName, Guid ID)
        {
            try
            {
                if (logicalName == String.Empty || ID == Guid.Empty)
                    return null;

                // Define a unique key for this method and its parameters.
                var key = $"Entities:{logicalName}-{ID}";

                Task<JObject> task = Task.Run(async () => await CRMRetrieval.GetEntityByIDAsync(key, logicalName, ID));

                task.Wait();

                var result = task.Result;

                if (Request == null)
                    Request = new HttpRequestMessage();

                return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message + " " + ex.InnerException, ex);
            }

            return null;
        } 

        // GET: api/CRMCache/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/CRMCache
        public void Post([FromBody]string value)
        {
            Get(value);
        }

        // PUT: api/CRMCache/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/CRMCache/5
        public void Delete(int id)
        {
        }

        public HttpResponseMessage DeleteAllKeys()
        {
            try
            {
                RedisSharedConnection.GetRedisServer().FlushDatabase();

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public HttpResponseMessage Delete(string key)
        {
            try
            {
                if (key == String.Empty)
                    return new HttpResponseMessage(HttpStatusCode.BadRequest); 

                // Define a unique key for this method and its parameters.
                // var key = $"Entities:{logicalName}";

                var cache = RedisSharedConnection.Connection.GetDatabase();

                // Try to get the entity from the cache.
                if (cache.KeyExists(key))
                {
                    bool wasDeleted = cache.KeyDelete(key);

                    if (wasDeleted)
                        return new HttpResponseMessage(HttpStatusCode.OK);
                }
                else
                {
                    return new HttpResponseMessage(HttpStatusCode.NotFound);
                }
                
            }
            catch (Exception ex)
            {
                throw;
            }

            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error");
        }
    }
}
