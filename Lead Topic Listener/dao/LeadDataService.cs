using System;
using System.Net.Configuration;
using System.Reflection;
using Microsoft.Xrm.Tooling.Connector;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;

using IFB.CRM.Services.Lead.connections;
using IFB.CRM.Services.Lead.models.amq;
using IFB.CRM.Services.Lead.models.crm.latebound;
using static System.String;

namespace IFB.CRM.Services.Lead.dao
{
	class LeadDataService
	{
		private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		private IOrganizationService _organizationService;

		public LeadDataService()
		{
			_organizationService = CrmServiceFactory.GetOrganizationService();
		}

		public void PushLeadToCrm(Entity lead)
		{
			try
			{
				var leadId = _organizationService.Create(lead);
				logger.Info($"Created Lead with ID: {leadId}");
			}
			catch (System.ServiceModel.FaultException e)
			{
				logger.Error(e.Message);
				logger.Error(e.Reason);
				logger.Error(e.InnerException);
				logger.Error(e.StackTrace);
				throw;
			}
		}

		public Entity BuildLeadFromContactUsModel(ContactUs ContactUsModel)
		{
			Entity lead = new Entity(LeadModel.EntityName);
			LeadModel.LeadSource_OptionSet leadSource;
			LeadModel.SourceCategory_OptionSet sourceCategory;
			LeadModel.SourceSubCategory_OptionSet sourceSubCategory;
			LeadModel.PreferredMethodofContact_OptionSet conactMethod;

			if (!String.IsNullOrEmpty(ContactUsModel.Source) 
			    && Enum.TryParse(ContactUsModel.Source.Replace(" ", ""), out leadSource))
				lead[LeadModel.LeadSource] = new OptionSetValue((int) leadSource);

			if (!String.IsNullOrEmpty(ContactUsModel.LeadSourceCategory) 
			    && Enum.TryParse(ContactUsModel.LeadSourceCategory.Replace(" ", ""), out sourceCategory))
				lead[LeadModel.SourceCategory] = new OptionSetValue((int) sourceCategory);

			if (!String.IsNullOrEmpty(ContactUsModel.FormSourceSubCategory) 
			    && Enum.TryParse(ContactUsModel.FormSourceSubCategory.Replace(" ", ""), out sourceSubCategory))
				lead[LeadModel.SourceSubCategory] = new OptionSetValue((int) sourceSubCategory);

			if (!String.IsNullOrEmpty(ContactUsModel.ContactMethod) 
			    && Enum.TryParse(ContactUsModel.ContactMethod.Replace(" ", ""), out conactMethod))
				lead[LeadModel.PreferredMethodofContact] = new OptionSetValue((int) conactMethod);

			lead[LeadModel.ExceedClientId] = IsNullOrEmpty(ContactUsModel.ExceedClientId) ? String.Empty : ContactUsModel.ExceedClientId;
			lead[LeadModel.ContactIdText] = IsNullOrEmpty(ContactUsModel.ContactIdText) ? String.Empty : ContactUsModel.ContactIdText;
			lead[LeadModel.FirstName] = IsNullOrEmpty(ContactUsModel.FirstName) ? String.Empty : ContactUsModel.FirstName;
			lead[LeadModel.LastName] = IsNullOrEmpty(ContactUsModel.LastName) ? String.Empty : ContactUsModel.LastName;
			lead[LeadModel.MobilePhone] = IsNullOrEmpty(ContactUsModel.Phone) ? String.Empty : ContactUsModel.Phone;
			lead[LeadModel.Email] = IsNullOrEmpty(ContactUsModel.Email) ? String.Empty : ContactUsModel.Email;
			lead[LeadModel.PolicyNumber] = IsNullOrEmpty(ContactUsModel.PolicyNumber) ? String.Empty : ContactUsModel.PolicyNumber;
			lead[LeadModel.Description] = IsNullOrEmpty(ContactUsModel.Comments) ? String.Empty : ContactUsModel.Comments;
			lead[LeadModel.AdditionalDetails] = IsNullOrEmpty(ContactUsModel.AdditonalDetails) ? String.Empty : ContactUsModel.AdditonalDetails;
			lead[LeadModel.PolicyType] = IsNullOrEmpty(ContactUsModel.PolicyType) ? String.Empty : ContactUsModel.PolicyType;
			lead[LeadModel.AgentFirstName] = IsNullOrEmpty(ContactUsModel.AgentFName) ? String.Empty : ContactUsModel.AgentFName;
			lead[LeadModel.AgentLastName] = IsNullOrEmpty(ContactUsModel.AgentLName) ? String.Empty : ContactUsModel.AgentLName;
			lead[LeadModel.AgentEmail] = IsNullOrEmpty(ContactUsModel.AgentEmail) ? String.Empty : ContactUsModel.AgentEmail;
			lead[LeadModel.AgentACID] = IsNullOrEmpty(ContactUsModel.AgentACID) ? ContactUsModel.AgentACID : ContactUsModel.AgentACID.Substring(1);
			lead[LeadModel.StreetOne] = IsNullOrEmpty(ContactUsModel.Street) ? String.Empty : ContactUsModel.Street;
			lead[LeadModel.City] = IsNullOrEmpty(ContactUsModel.City) ? String.Empty : ContactUsModel.City;
			lead[LeadModel.State_Province] = IsNullOrEmpty(ContactUsModel.State) ? String.Empty : ContactUsModel.State;
			lead[LeadModel.ZipCode] = IsNullOrEmpty(ContactUsModel.Zip) ? String.Empty : ContactUsModel.Zip;
			lead[LeadModel.PolicyStreet] = IsNullOrEmpty(ContactUsModel.PolicyStreet) ? String.Empty : ContactUsModel.PolicyStreet;
			lead[LeadModel.PolicyCity] = IsNullOrEmpty(ContactUsModel.PolicyCity) ? String.Empty : ContactUsModel.PolicyCity;
			lead[LeadModel.PolicyState] = IsNullOrEmpty(ContactUsModel.PolicyState) ? String.Empty : ContactUsModel.PolicyState;
			lead[LeadModel.PolicyZip] = IsNullOrEmpty(ContactUsModel.PolicyZip) ? String.Empty : ContactUsModel.PolicyZip;
			lead[LeadModel.EventDate] = IsNullOrEmpty(ContactUsModel.EventDate) ? String.Empty : ContactUsModel.EventDate;
			lead[LeadModel.EventTime] = IsNullOrEmpty(ContactUsModel.EventTime) ? String.Empty : ContactUsModel.EventTime;
			lead[LeadModel.EventAM_PM] = IsNullOrEmpty(ContactUsModel.EventAmPm) ? String.Empty : ContactUsModel.EventAmPm;
			lead[LeadModel.Topic] = $"{String.Format("{0:yyyy/M/d}", DateTime.Now)}|{ContactUsModel.FirstName}" + 
			                        $"|{ContactUsModel.LastName}|{ContactUsModel.LeadSourceCategory}";

			if (!String.IsNullOrEmpty(ContactUsModel.Apt))
				lead[LeadModel.StreetOne] = $"{lead[LeadModel.StreetOne]} {ContactUsModel.Apt}";

			if (!String.IsNullOrEmpty(ContactUsModel.PolicyApt))
				lead[LeadModel.PolicyStreet] = $"{lead[LeadModel.PolicyStreet]} {ContactUsModel.PolicyApt}";

			return lead;

		}
	}
}
