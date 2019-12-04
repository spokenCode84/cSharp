using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CRM.Helper;
using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebAPI.CRMHelpers
{
    public static class CRMRetrieval
    {
        // Set 24 hour expiration as a default
        private const double DefaultExpirationTimeInMinutes = 1440.0;

        public static async Task<JObject> GetAllRecordsForAnEntityAsync(string key, string entityLogicalName)
        {
            var cache = RedisSharedConnection.Connection.GetDatabase();

            // Try to get the entity from the cache.
            var json = await cache.StringGetAsync(key).ConfigureAwait(false);

            var value = string.IsNullOrWhiteSpace(json)
                 ? default(JObject)
                 : JsonConvert.DeserializeObject<JObject>(json);

            if (value == null) // Cache miss
            {
                // If there's a cache miss, get the entity from the original store and cache it.
                // Code has been omitted because it is data store dependent.

                // string configpath = Environment.CurrentDirectory + "\\web.config";

                string connectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;

                APIOperations operations = new APIOperations();
                operations.ConnectToCRMWithConnectionString(connectionString);
                operations.getWebAPIVersion().Wait();

                Task<JObject> returnVal;
                
                // get All
                returnVal = Task.Run(async () => await operations.GetAllRecordsForAnEntity(entityLogicalName, String.Empty));
                
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

        public static async Task<JObject> GetEntityByIDAsync(string key, string entityLogicalName, Guid recordID)
        {
            var cache = RedisSharedConnection.Connection.GetDatabase();

            // Try to get the entity from the cache.
            var json = await cache.StringGetAsync(key).ConfigureAwait(false);

            var value = string.IsNullOrWhiteSpace(json)
                 ? default(JObject)
                 : JsonConvert.DeserializeObject<JObject>(json);

            if (value == null) // Cache miss
            {
                // If there's a cache miss, get the entity from the original store and cache it.
                // Code has been omitted because it is data store dependent.

                // string configpath = Environment.CurrentDirectory + "\\web.config";

                string connectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;

                APIOperations operations = new APIOperations();
                operations.ConnectToCRMWithConnectionString(connectionString);
                operations.getWebAPIVersion().Wait();

                Task<JObject> returnVal;
                
                returnVal = Task.Run(async () => await operations.GetEntityByID(entityLogicalName, String.Empty, recordID));
                
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