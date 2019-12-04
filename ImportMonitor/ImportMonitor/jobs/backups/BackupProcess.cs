using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using ImportMonitor.Common;
using ImportMonitor.Common.Data;

namespace ImportMonitor.Jobs
{
    class BackupProcess: Job
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int DAYS_TO_KEEP;
        private DateTime CurrentDay;

        public BackupProcess()
        {
            this.DAYS_TO_KEEP = Env.DaysToKeep;
            this.CurrentDay = DateTime.Today;
        }
        public void DeleteBackups()
        {
            log.Info("Starting Job: Backups");
            foreach(ImportProcess import in MyServer.Processes)
            {
                import.DirProcessed = import.DirProcessed.Replace("D:\\", $"\\\\{MyServer.ServerName}\\");
              //  float FolderSize = Util.CalculateFolderSize(import.DirProcessed);
                log.Info($"Initializing cleanup of {import.ProcessDisplayName}.");
                log.Info($"Looking for backup directory: {import.DirProcessed}");
                if (Directory.Exists(import.DirProcessed))
                {
                    log.Info($"Found directory: {import.DirProcessed}");
                    DirectoryInfo BackupDir = new DirectoryInfo(import.DirProcessed);                  
                    DirectoryInfo[] BackupFolders = BackupDir.GetDirectories();
                    int BackupFoldersCnt = BackupFolders.Count();
                    log.Info($"Found {BackupFoldersCnt} backup folders");
                    if (BackupFoldersCnt > 0)
                    {   
                        for (int i = 0; i < BackupFoldersCnt; i++)
                        {
                            int DAYS_OLD = (this.CurrentDay - BackupFolders[i].LastWriteTime).Days;
                            if(DAYS_OLD > this.DAYS_TO_KEEP)
                            {
                                log.Info($"DELETING FOLDER: {BackupFolders[i].Name} >>>---> {DAYS_OLD} days old.");
                               // BackupFolders[i].Delete();
                            } 
                            else   
                            {
                                log.Info($"KEEPING FOLDER: {BackupFolders[i].Name} >>>---> {DAYS_OLD} days old.");
                            }         
                        }
                    }
                    else
                    {
                        log.Info($"No backup folders for {import.ProcessDisplayName}");
                    }
                }
                else
                {
                    log.Warn($"No backup folder found for {import.ProcessDisplayName}." +
                                $"Check configuration: {this.ProcessErrReport(import)}");
                }
            }
        }
    }
}
