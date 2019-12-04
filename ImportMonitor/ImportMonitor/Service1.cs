using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using ImportMonitor.Common;
using ImportMonitor.Jobs;

namespace ImportMonitor
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            if (Env.ResetCache) 
            {
                CacheManager cache = new CacheManager();
                cache.LoadUtilServerProcesses();
                cache.WriteServerConfigurationToFile();
            }
            if (Env.RunJobs["redrops"])
            {
                ErrorMonitor IRerrorMonitor = new ErrorMonitor();
              //  redrop_job.Initialize();
                
            } else
            {
                // no redrop
            }
            if (Env.RunJobs["backups"])
            {
                BackupProcess BackupSweep = new BackupProcess();
                BackupSweep.Initialize();
                BackupSweep.DeleteBackups();
            } else
            {
                // No backup.
            }
        }

        protected override void OnStop()
        {
        }

        public void Start()
        {
            OnStart(new string[0]);
        }
    }
}
