using System;

using Apache.NMS;
using IFB.CRM.Services.Lead.handlers;

namespace IFB.CRM.Services.Lead.subscriptions
{
	class Subscription: ISubscription
	{
		private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		public string URI { get; set; }
		public string ClientId { get; set; }
		public string userName { get; set; }
		public string password { get; set; }
		public string destination { get; set; }
		public IHandler handler { get; set; }


		public Subscription(String uri, String clientId, String userName, String password, String destination, IHandler handler)
		{
			this.URI = uri;
			this.ClientId = clientId;
			this.userName = userName;
			this.password = password;
			this.destination = destination;
			this.handler = handler;
		}

		public Boolean Listen()
		{
			NMSConnectionFactory factory = new NMSConnectionFactory($"activemq:failover:({this.URI})");

			try
			{
				using (IConnection connection = factory.CreateConnection(this.userName, this.password))
				{
					connection.ClientId = this.ClientId;
					connection.Start();
					using (ISession session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge))
					using (IDestination dest = session.GetQueue(this.destination))
					using (IMessageConsumer consumer = session.CreateConsumer(dest))
					{
						IMessage message = consumer.Receive();

						if (message is ITextMessage)
						{
							ITextMessage txtMessage = message as ITextMessage;
							logger.Debug("Received " + txtMessage.Text);
							this.handler.ProcessMessage(txtMessage.Text);
							return true;
						}
						else
						{
							logger.Error($"Unexpected Msg Type {message.ToString()}");
						}

					}
					return false;
				}
			}
			catch (NMSException e)
			{
				logger.Error($"Unable to connect to Active MQ {this.URI}");
				logger.Error($"Message: {e.Message}");
				logger.Error($"Inner Exception {e.InnerException}");
				return false;
			}
		}

	}
}
