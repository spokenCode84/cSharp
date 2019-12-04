using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using log4net;

using ImportMonitor.Common.Data;
using System.Configuration;
using System.IO;
using Newtonsoft.Json;

namespace ImportMonitor.Common
{
    class CacheManager
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public List<UtilServer> Servers;
        public ImportCache IRData;
        public Dictionary<String, String> ProcessKeyNameMap { get; set; }

        public CacheManager()
        {
           this.Servers = new List<UtilServer>(); 
           this.IRData = new ImportCache();
           this.ProcessKeyNameMap = new Dictionary<string, string>();
           this.ProcessKeyNameMap.Add("Enabled", ConfigurationManager.AppSettings["IMPORT_PROCESS"]);
           this.ProcessKeyNameMap.Add("ProcessDisplayName", ConfigurationManager.AppSettings["IMPORT_PROCESS"]);
           this.ProcessKeyNameMap.Add("ProcessDescription", ConfigurationManager.AppSettings["IMPORT_PROCESS"]);
           this.ProcessKeyNameMap.Add("ProcessingDirName", ConfigurationManager.AppSettings["FS_CXN"]);
           this.ProcessKeyNameMap.Add("SearchPattern", ConfigurationManager.AppSettings["FS_CXN"]);
           this.ProcessKeyNameMap.Add("DirProcessed", ConfigurationManager.AppSettings["FS_CXN"]);
           this.ProcessKeyNameMap.Add("DirImport", ConfigurationManager.AppSettings["FS_CXN"]);
           this.ProcessKeyNameMap.Add("DirError", ConfigurationManager.AppSettings["FS_CXN"]);
           this.ProcessKeyNameMap.Add("Type", ConfigurationManager.AppSettings["FS_INTERACTION"]); 
        }

        public void LoadUtilServerProcesses()
        {
            foreach (var util in Env.UtilServers)
            {
                UtilServer UtilCache = new UtilServer(util["name"]);
                List<String> ParentIds = new List<String>();
                ParentIds = this.IRData.GetParentIdsByServer(util["keyname"]);
                log.Debug($"Initializnig Cache Load: {util["name"]}");                              
                if (ParentIds.Count > 0)
                {
                    foreach (var id in ParentIds)
                    {
                        ImportProcess process = this.DeconstructImportProcess(id);
                       // if(Env.DEBUG) { log.Info($"ParentId: {id}"); }
                        if(process is ImportProcess)
                        {
                            UtilCache.Processes.Add(process); 
                        } else
                            {
                             log.Error($"Could not build Import Process DAO for Parent ID: {id}");
                            }
                    } 
                } else
                    {
                        log.Error($"No Parent IDs returned by query on{util["name"]}");
                    }
                if (UtilCache.Processes.Count > 0) { this.Servers.Add(UtilCache); }             
            }
        }

        public void WriteServerConfigurationToFile()
        {
            /* To Do: 
                    1) Move to Util Class with parameter for Servers List.
                    2) Check to see if file exists, and if it does, delete/override it.
             */
            File.WriteAllText(Env.CACHE_DIR + "\\" + Env.AppServer["name"] + ".json", JsonConvert.SerializeObject(this.Servers));
        }

        public ImportProcess DeconstructImportProcess(String id)
        {
            ImportProcess ProcessDefinition = new ImportProcess();
            var ImportProcessFields = ProcessDefinition.GetType().GetProperties();       
            foreach (var field in ImportProcessFields)
            {
                field.SetValue(ProcessDefinition, this.SetProcessAttribute(id, field.Name, this.ProcessKeyNameMap[field.Name]));
            }
            return ProcessDefinition;
        }

        public String SetProcessAttribute(String id, String lookup, String keyname)
        {
            String matchLookup = $"<{lookup}>.*</{lookup}>";
            String exceptLookup = $"</?{lookup}>";
            byte[] BinaryConfiguration = this.IRData.GetBinaryProcessConfig(id, keyname);
            String XMLConfiguration = System.Text.Encoding.UTF8.GetString(BinaryConfiguration);
            return Regex.Replace(Regex.Match(XMLConfiguration, @matchLookup).ToString(), @exceptLookup, "");
        }

        public void DisplaySecurity()
        {
            byte[] BinaryConfiguration = this.IRData.GetBinarySecurityConfig();
            String XMLConfiguration = System.Text.Encoding.UTF8.GetString(BinaryConfiguration);
            Console.WriteLine("AARON");   
            Console.WriteLine(XMLConfiguration);
        }
        
    }
}

