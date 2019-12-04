using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace WebAPI
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

            var options = ConfigurationOptions.Parse(cacheConnection);
            options.ConnectRetry = 5;
            options.AllowAdmin = true;

            return ConnectionMultiplexer.Connect(options);
        });

        public static ConnectionMultiplexer Connection => lazyConnection.Value;

        public static IServer GetRedisServer()
        {
            return lazyConnection.Value.GetServer(ConfigurationManager.AppSettings["ReddisServerName"]);
        }

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