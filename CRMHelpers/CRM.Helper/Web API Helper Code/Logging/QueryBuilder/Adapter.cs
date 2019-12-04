using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;

namespace CRM.Helper.Web_API_Helper_Code.Logging.QueryBuilder
{
	public abstract class Adapter
	{

		private static string LOG_PREFIX = ConfigurationManager.AppSettings["LoggingEnvCxnPrefix"];
		private static string ENV = Environment.GetEnvironmentVariable("CRM_LOG_ENV")?.ToLower();
		private static string ENV_BACKUP = ConfigurationManager.AppSettings["CRM_LOG_ENV_BACKUP"]?.ToLower();
		public string dbName; 
		public SqlConnection connection;
		public SqlDataReader reader;

		public Adapter() {
			this.connection = new SqlConnection(ConnectionBuilder.ConnectionBuilder.GetDatabaseCxnString());
			this.dbName = "CRM_Logs";
		}

	}
}
