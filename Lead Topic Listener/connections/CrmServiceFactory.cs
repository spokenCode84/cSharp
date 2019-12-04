using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;

using IFB.CRM.Services.Lead.models.env;

namespace IFB.CRM.Services.Lead.connections
{
	public static class CrmServiceFactory
	{

		public static String OrganizationConnectionString
		{
			get { return ConfigurationManager.ConnectionStrings[Env.CurrentRegion].ConnectionString; }
		}

		public static IOrganizationService GetOrganizationService()
		{
			CrmServiceClient connection = new CrmServiceClient(CrmServiceFactory.OrganizationConnectionString);

			if (connection.OrganizationServiceProxy != null)
				connection.OrganizationServiceProxy.Timeout = new TimeSpan(0, 10, 0);

			IOrganizationService orgService = connection.OrganizationWebProxyClient != null ?
				(IOrganizationService)connection.OrganizationWebProxyClient :
				(IOrganizationService)connection.OrganizationServiceProxy;

			return orgService;
		}
	}
}

