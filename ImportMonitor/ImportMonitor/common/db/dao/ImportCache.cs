using System;
using System.Text;
using System.Linq;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;

using log4net;
using ImportMonitor.Common;


namespace ImportMonitor.Common.Data
{
    class ImportCache : Adapter
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // public ImportCache()
        // {
        //     this.connection = new SqlConnection(Adapter.DataSource);
        // }

        public List<String> GetParentIdsByServer(String server)
        {
            List<String> parentIds = new List<String>();
            this.OpenAndExecute(String.Format(Queries.GetParentIdsByServer, server));
            while(this.data.Read()) { parentIds.Add(this.data.GetValue(0).ToString()); }
            this.Close();
            return parentIds;
        }

        public byte[] GetBinaryProcessConfig(String id, String keyname)
        {
            byte[] binary_config = new byte[0];
			Console.WriteLine(String.Format(Queries.GetBinaryProcessConfig, id, keyname));

			this.OpenAndExecute(String.Format(Queries.GetBinaryProcessConfig, id, keyname));
            while (this.data.Read()) {
				Console.WriteLine(this.data.GetValue(0));
				binary_config = (byte[])this.data.GetValue(0);
			}
            this.Close();
            return binary_config;
        }

        public byte[] GetBinarySecurityConfig()
        {
            byte[] binary_config = new byte[0];
            this.OpenAndExecute(@"SELECT permissions from ObjectType WHERE typeid in ('235')");
            while (this.data.Read()) { binary_config = (byte[])this.data.GetValue(0);}
            this.Close();
            return binary_config;
        }

        
        static class Queries
        {
            public static String GetParentIdsByServer =
            @"SELECT DISTINCT parentid
            FROM SystemConfig
            WHERE parentid IN
            (SELECT configid
            FROM SystemConfig
            WHERE parentid
            IN (SELECT parentid
                FROM SystemConfig
                WHERE parentid
                IN (SELECT configid 
                    FROM SystemConfig
                    WHERE keyname 
                    LIKE '%{0}%'
                    )
                )
            )";

            public static String GetBinaryProcessConfig = 
            @"SELECT keyvalue 
            FROM SystemConfig
            WHERE parentid = {0}
            AND keyname = '{1}'";
        }
    }
}
