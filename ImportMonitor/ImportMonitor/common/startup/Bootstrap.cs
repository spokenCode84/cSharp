using System;
using System.IO;
using System.Web;
using System.Linq;
using System.Collections.Generic;

using log4net;
using Newtonsoft.Json;


namespace ImportMonitor.Common
{
    public static class Bootstrap
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void LoadEnvironmentConfiguration()
        {   
            String env = Environment.GetEnvironmentVariable("IMPORT_REGION");
            try
            {
                JsonConvert.DeserializeObject<Env>(File.ReadAllText(Env.CFG_DIR + $"\\{env.ToLower()}.json"));  
                log.Info(Env.CFG_DIR);         
            }
            catch(IOException e)
            {
                log.Error($"Cannot find JSON config file in: {Env.CFG_DIR}");
                log.Error(e.Message);
            }
        }


        public static void LoadEnvironmentDataSource()
        {
            Adapter.SetConnection();       
        }

    }
}