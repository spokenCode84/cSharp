using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

using log4net;

namespace IFB.CRM.Services.Lead
{
	static class Program
	{
		private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		static void Main()
		{
			logger.Debug("Initializing Environment Configuration");
			Bootstrap.LoadEnvironmentConfiguration();

			#if DEBUG
				Service1 myService = new Service1();
				myService.OnDebug();
				System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
			#else
				ServiceBase[] ServicesToRun;
				ServicesToRun = new ServiceBase[]
				{
					new Service1()
				};
				ServiceBase.Run(ServicesToRun);
			#endif
		}



	}
}
