using System;
using System.ServiceProcess;
using System.Threading;

namespace IFB.CRM.Services.Lead
{
	public partial class Service1 : ServiceBase
	{

		private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		private Thread _job;
		private DateTime? _stopRequestedTime;
		private Listener listener;

		// https://ifbdmz-qa.infarmbureau.com/customer-service/contact-us

		public Service1()
		{
			InitializeComponent();
		}

		public void OnDebug()
		{
			OnStart(null);
		}

		protected override void OnStart(string[] args)
		{
			try
			{
				listener = new Listener();
				_job = new Thread(new ThreadStart(listener.ProcessMessages));
				_job.Start();
			}
			catch (Exception e)
			{
				logger.Error(e.Message);
				logger.Error(e.StackTrace);
				logger.Error(e.InnerException);
				logger.Error(e.InnerException);
			}
		}

		protected override void OnStop()
		{
			_stopRequestedTime = DateTime.Now;
			listener.isStopping = true;

			while (listener.isStopping.Value && DateTime.Now.Subtract(_stopRequestedTime.Value).Seconds < 45)
			{
				Thread.Sleep(1000);
			}

			if (listener.isStopping.Value)
			{
				_job.Abort();
			}
		}

	}
}
