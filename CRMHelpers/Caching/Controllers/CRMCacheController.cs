using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using CRM.Helper;
using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;

namespace CRMHelper.API.Controllers
{
    // https://docs.microsoft.com/en-us/azure/architecture/patterns/cache-aside

    public class CRMCacheController : ApiController
    {
        // Set five minute expiration as a default
        private const double DefaultExpirationTimeInMinutes = 5.0;
        
        APIOperations operations = new APIOperations();

        //  ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");

        // GET: api/CRMCache
        // Example: http://localhost:53767/api/CRMCache?logicalName=ifb_externalsystemlinkses
        public HttpResponseMessage Get(string logicalName)
        {
            if (logicalName == String.Empty)
                return null;

            // Define a unique key for this method and its parameters.
            var key = $"Entities:{logicalName}";

            Task<string> task = Task.Run(async () => await GetMyEntityAsync(key, logicalName, Guid.Empty));

            task.Wait();

            var result = task.Result;

            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }

        public String GetEntityByID(string logicalName, Guid ID)
        {
            if (logicalName == String.Empty || ID == Guid.Empty)
                return null;

            // Define a unique key for this method and its parameters.
            var key = $"Entities:{logicalName}-{ID}";

            Task<string> task = Task.Run(async () => await GetMyEntityAsync(key, logicalName, ID));

            task.Wait();

            var result = task.Result;

            return result;
        } 

        // GET: api/CRMCache/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/CRMCache
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/CRMCache/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/CRMCache/5
        public void Delete(int id)
        {
        }

        
        public async Task<JObject> GetMyEntityAsync(string key, string entityLogicalName, Guid recordID)
        {
             var cache = RedisSharedConnection.Connection.GetDatabase();

            // Try to get the entity from the cache.
            var json = await cache.StringGetAsync(key).ConfigureAwait(false);

            var value = string.IsNullOrWhiteSpace(json) ? 
                string.Empty : 
                (string)json;

           //     ? default(Entity)
           //     : JsonConvert.DeserializeObject<Entity>(json);

            if (value == string.Empty) // Cache miss
            {
                // If there's a cache miss, get the entity from the original store and cache it.
                // Code has been omitted because it is data store dependent.
                
                // string configpath = Environment.CurrentDirectory + "\\web.config";

                string connectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;

                operations.ConnectToCRMWithConnectionString(connectionString);

                operations.getWebAPIVersion().Wait();

                Task<JObject> returnVal;

                //value = operations.CreateWithAssociationAsync()
                if (recordID == Guid.Empty)
                {
                    // get All
                    returnVal = Task.Run(async () => await operations.GetEntity(entityLogicalName, String.Empty));
                }
                else
                {
                    returnVal = Task.Run(async () => await operations.GetEntityByID(entityLogicalName, String.Empty, recordID));
                }


                returnVal.Wait();

                value = returnVal.Result;

                // Avoid caching a null value.
                if (value != null)
                {
                    // Put the item in the cache with a custom expiration time that
                    // depends on how critical it is to have stale data.

                    await cache.StringSetAsync(key, JsonConvert.SerializeObject(value)).ConfigureAwait(false);

                    //await cache.StringSetAsync(key, value).ConfigureAwait(false);
                    await cache.KeyExpireAsync(key, TimeSpan.FromMinutes(DefaultExpirationTimeInMinutes)).ConfigureAwait(false);
                }
            }

            return value;
        }

    }
}
