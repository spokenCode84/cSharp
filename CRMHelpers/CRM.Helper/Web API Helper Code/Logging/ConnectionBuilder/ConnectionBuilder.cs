using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace CRM.Helper.Web_API_Helper_Code.Logging.ConnectionBuilder
{
	public static class ConnectionBuilder
	{
		private static string LOG_PREFIX = ConfigurationManager.AppSettings["LoggingEnvCxnPrefix"];
		private static string ENV = Environment.GetEnvironmentVariable("CRM_LOG_ENV")?.ToLower();
		private static string ENV_BACKUP = ConfigurationManager.AppSettings["CRM_LOG_ENV_BACKUP"]?.ToLower();


		public static string GetDatabaseCxnString()
		{
			StringBuilder sbConnectionStringToUse = new StringBuilder(LOG_PREFIX);

			if (ENV != null)
				sbConnectionStringToUse.Append(ENV);
			else if (ENV_BACKUP != null)
				sbConnectionStringToUse.Append(ENV_BACKUP);
			else
				throw new Exception("No Environment Found");

			string connectionString = ConfigurationManager.ConnectionStrings[sbConnectionStringToUse.ToString()].ConnectionString;

			return connectionString;
		}

	}
}
