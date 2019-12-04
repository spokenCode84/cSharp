using System;
using System.Linq;
using System.Reflection;
using Microsoft.Xrm.Tooling.Connector;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;

using Apache.NMS.ActiveMQ.Commands;
using IFB.CRM.Services.Lead.connections;
using IFB.CRM.Services.Lead.dao;
using Newtonsoft.Json;

using IFB.CRM.Services.Lead.models.amq;
using IFB.CRM.Services.Lead.models.crm.latebound;

namespace IFB.CRM.Services.Lead.handlers
{
	public class ContactUsHandler : IHandler
	{
		private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		public void ProcessMessage(String text)
		{
			logger.Debug($"Processing incoming message: {text}");

			try
			{
				ContactUs ContactUsModel = JsonConvert.DeserializeObject<ContactUs>(text);
				LeadDataService LeadService = new LeadDataService();
				Entity lead = LeadService.BuildLeadFromContactUsModel(ContactUsModel);

				try
				{
					LeadService.PushLeadToCrm(lead);
				}
				catch (System.ServiceModel.FaultException e)
				{
					logger.Error($"Error Creating Lead from {ContactUsModel.ToString()} ");
					logger.Error($"Message: {e.Message}");
					throw;
				}
			}
			catch (JsonException e)
			{
				logger.Error($"Invalid Contact Us Message: {text}");
				logger.Error($"Message {e.Message}");
				logger.Error($"Inner Exception: {e.InnerException}");
				throw;
			}
		}
	}
}