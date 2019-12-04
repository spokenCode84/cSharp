using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using WebAPI.Controllers;

namespace WebAPI.Models
{
    public class CacheViewModel
    {
        public Dictionary<string, string> redisDictionary { get; set; }

        private void GetAllKeysFromRedis()
        {
            var keys = RedisSharedConnection.GetRedisServer().Keys();

            var cachedb = RedisSharedConnection.Connection.GetDatabase();

            string[] keysArr = keys.Select(key => (string)key).ToArray();

            foreach (var key in keysArr)
            {
                var task = Task.Run(async () => await cachedb.StringGetAsync(key).ConfigureAwait(false));

                task.Wait();

                redisDictionary.Add(key, task.Result);

                // cachedb.StringGet(key);
            }
        }

        public CacheViewModel()
        {
            redisDictionary = new Dictionary<string, string>();
            GetAllKeysFromRedis();
        }

        public HttpResponseMessage AddEntityToCache(string entityName)
        {
            CRMCacheController controller = new CRMCacheController();
            return controller.Get(entityName);
        }

        public HttpResponseMessage DeleteFromCache(string key)
        {
            CRMCacheController controller = new CRMCacheController();
            return controller.Delete(key);
        }

        public HttpResponseMessage DeleteAllFromCache()
        {
            CRMCacheController controller = new CRMCacheController();
            return controller.DeleteAllKeys();
        }
    }
}