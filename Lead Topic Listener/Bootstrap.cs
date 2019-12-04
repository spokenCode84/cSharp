using System;
using System.IO;
using System.Web;
using System.Linq;
using System.Collections.Generic;

using log4net;
using Newtonsoft.Json;
using IFB.CRM.Services.Lead.models.env;
using IFB.CRM.Services.Lead.connections;

namespace IFB.CRM.Services.Lead
{
	public static class Bootstrap
	{
		private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		public static void LoadEnvironmentConfiguration()
		{
			String env = Env.CurrentRegion;
#if DEBUG
			env = "DEV";
#endif

			try
			{
				JsonConvert.DeserializeObject<Env>(File.ReadAllText($"{Env.ConfigDir}\\{env}.json"));
			}
			catch (IOException e)
			{
				logger.Error($"Cannot find JSON config file in: {Env.ConfigDir}");
				logger.Error(e.Message);
			}
		}

		public static void SetupCRMConnectionParameters()
		{

		}


	}
}
