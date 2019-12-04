using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using IFB.CRM.Services.Lead.subscriptions;
using IFB.CRM.Services.Lead.models.env;
using IFB.CRM.Services.Lead.handlers;


namespace IFB.CRM.Services.Lead
{
	class Listener
	{
		private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		public Boolean? isStopping { get; set; }

		public void ProcessMessages()
		{
			isStopping = false;

			try
			{
				Subscription contactUsListener = new Subscription(Env.ActiveMq["uri"], Env.ActiveMq["clientId"],
					Env.ActiveMq["userName"], Env.ActiveMq["password"], Env.queues["newLead"], new ContactUsHandler());

				while (!isStopping.Value && contactUsListener.Listen())
				{
					try
					{
						logger.Debug("Successfully read message");
					}
					catch (Exception e)
					{
						logger.Error(e.Message);
					}
				}
			}
			catch (Exception e)
			{
				logger.Error(e.Message);
			}

			isStopping = null;
		}
	}
}
