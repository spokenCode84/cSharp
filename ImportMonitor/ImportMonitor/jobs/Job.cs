using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using log4net;
using Newtonsoft.Json;

using ImportMonitor.Common;
using ImportMonitor.Common.Data;

/*
|--------------------------------------------------------------------------------------
| Abstract Class: Jobs
|--------------------------------------------------------------------------------------
|
|   Purpose: This class deserializes, identifies, and sets the MyServer
|            object used by Jobs inheriting from it.
|
|
|   Properties:
|       
|       EnvServers - List of UtilServer objects loaded from cache directory.
|       MyServer - The UtilServer object that matches the local machine's name.
|
|   Methods:
|
|       Initialize -  Deserializes json file from cache directory into EnvServers list.
|                     This will match the UtilServer object where ServerName matches
|                     the local machine's name, and other UtilServer objects are deleted.
|                     It then sets MyServer to the selected UtilServer object.
|
|       ProcessErrReport - StringBuilder to display ImportProcess object details.
|
|
*/

namespace ImportMonitor.Jobs
{
    abstract class Job : IJob
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public List<UtilServer> EnvServers;
        public UtilServer MyServer;

        public void Initialize()
        {
            List<bool> RemoveServer = new List<bool>();
            String LocalServerName = Env.DEBUG ? "ap-imgutil2-dev" : System.Environment.MachineName;
            try 
            {
                this.EnvServers = JsonConvert.DeserializeObject<List<UtilServer>>(File.ReadAllText(Env.CACHE_DIR + $"\\{Env.AppServer["name"]}.json"));
                log.Info("Deserialized Server configuration.");
            } 
            catch(IOException e)
            {
                log.Error($"Cannot find JSON config file in: {Env.CACHE_DIR}");
                log.Error(e.Message);
            }
            foreach (UtilServer server in this.EnvServers)
            {
                if (server.ServerName != LocalServerName)
                {
                    RemoveServer.Add(true);
                }
                else
                {
                    RemoveServer.Add(false);
                }
            }
            for (int i = 0; i < RemoveServer.Count; i++)
            {
                if (RemoveServer[i])
                {
                    log.Info("Deleting remote server config: " + this.EnvServers[i].ServerName);
                    this.EnvServers.RemoveAt(i);
                }
            }
            if(this.EnvServers.Count == 1)
            {
                this.MyServer = this.EnvServers[0];
                log.Info("Match: " +  this.MyServer.ServerName+ "Instantiating Local Server Object.");
            }
            else
            {
                log.Error("Zero, or multiple servers found. \n" +
                           "Unable to instantiate local server object.");
            }
        }

        protected String ProcessErrReport(ImportProcess import)
        {
            StringBuilder report = new StringBuilder();
            report.Append($"\n\t{import.ProcessDisplayName}\n");
            report.Append($"\t{import.DirError}\n");
            report.Append($"\t{import.DirImport}\n");
            report.Append($"\t{import.DirProcessed}\n");
            report.Append($"\t{import.ProcessDescription}\n");
            return report.ToString();
        }
    }
}
