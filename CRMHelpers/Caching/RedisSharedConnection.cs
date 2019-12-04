using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace CRMHelper.API
{
    public class RedisSharedConnection
    {
        private static ConnectionMultiplexer _Connection;
        
        // Set five minute expiration as a default
        private const double DefaultExpirationTimeInMinutes = 5.0;

        // Redis Connection string info
        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            string cacheConnection = ConfigurationManager.AppSettings["RedisConnectionString"].ToString();
            return ConnectionMultiplexer.Connect(cacheConnection);
        });

        public static ConnectionMultiplexer Connection => lazyConnection.Value;
        

        //private static Lazy<ConfigurationOptions> configOptions
        //    = new Lazy<ConfigurationOptions>(() =>
        //    {
        //        var configOptions = new ConfigurationOptions();
        //        configOptions.EndPoints.Add("localhost:53767");
        //        configOptions.ClientName = "SafeRedisConnection";
        //        configOptions.ConnectTimeout = 100000;
        //        configOptions.SyncTimeout = 100000;
        //        configOptions.AbortOnConnectFail = false;
        //        return configOptions;
        //    });

        //private static Lazy<ConnectionMultiplexer> conn
        //    = new Lazy<ConnectionMultiplexer>(
        //        () => ConnectionMultiplexer.Connect(configOptions.Value));

        //private static ConnectionMultiplexer SafeConn
        //{
        //    get
        //    {
        //        return conn.Value;
        //    }
        //}
    }
}