using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using log4net;
using ImportMonitor.Common;
using ImportMonitor.Common.Data;

namespace ImportMonitor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main()
        {
            
            log.Debug("Initializing Environment Configuration");
            Bootstrap.LoadEnvironmentConfiguration();
            Bootstrap.LoadEnvironmentDataSource();
            
            if (Env.DEBUG)
            {
                var myService = new Service1();
                log.Debug("Starting service...");
                log.Debug("Machine Name: " + System.Environment.MachineName);
                myService.Start();
                log.Debug("Service is running in DEBUG mode.");
                Console.ReadLine();
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] { new Service1() };
                log.Debug("Starting service...");
                ServiceBase.Run(ServicesToRun);
                log.Debug("Service is running in PRODUCTION mode.");
            }
        }
    }
}
