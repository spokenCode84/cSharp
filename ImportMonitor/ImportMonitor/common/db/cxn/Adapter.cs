using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;

using log4net;

using ImportMonitor.Common;
using log4net.Repository.Hierarchy;

namespace ImportMonitor.Common
{
    public abstract class Adapter
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        // TO DO: Make this into interface IAdapter,
        //        Abstract Class adapter with open
        //        and close methods.  And let the
        //        concrete classes define SetConnection.
        //        Would allow this component to be reusalbe,
        //        and connect to other data sources. Might
        //        have to define the cmd, data, connection
        //        variables in concrete classes for DB2 and
        //        and SQL Server.  Might not be able to 
        //        declare SetConnection in Interface, but 
        //        instead declare in abstract class as static.
        public static String DataSource;
        protected String query;
        protected SqlCommand cmd;
        protected SqlDataReader data;
        protected SqlConnection connection;

        public static void SetConnection()
        {
            Adapter.DataSource = ConfigurationManager.ConnectionStrings["IR_" + Env.DataSource["suffix"]].ToString();
            if (Adapter.DataSource.Length <= 0)
            {
                log.Error($"Unable to build Connection String. \n Check {Env.DataSource["suffix"]}, and App.config");
            }
        }

        protected void OpenAndExecute(String query)
        {
	        try
	        {
		        this.connection = new SqlConnection(Adapter.DataSource);
		        this.connection.Open();
		        this.data = new SqlCommand(query, this.connection).ExecuteReader();
	        }
	        catch (Exception ex)
	        {
		        log.Error(ex.Message);
	        }
        }

        protected void Close()
        {
            this.connection.Close();
        }

        
    }
}
