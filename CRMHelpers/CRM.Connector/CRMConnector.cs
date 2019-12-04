using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Tooling.Connector;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using Microsoft.Crm.Sdk.Messages;

namespace CRM.Connector
{
    public static class CRMConnector
    {
        public static void CRMConnectorTest()
        {
            var sw = new Stopwatch();

            sw.Start();

            var crmSvc = GetCrmClient();

            Console.WriteLine($"Time to get Client # 1: {sw.ElapsedMilliseconds}");

            crmSvc.OrganizationServiceProxy.Execute(new WhoAmIRequest());

            Console.WriteLine($"Time to WhoAmI # 1: {sw.ElapsedMilliseconds}");

            var crmSvc2 = GetCrmClient();

            Console.WriteLine($"Time to get Client # 2: {sw.ElapsedMilliseconds}");

            crmSvc2.OrganizationServiceProxy.Execute(new WhoAmIRequest());

            Console.WriteLine($"Time to WhoAmI # 2: {sw.ElapsedMilliseconds}");

        }

        public static CrmServiceClient GetCrmClient()
        {
            return new CrmServiceClient(ConfigurationManager.ConnectionStrings["CRMDev"].ConnectionString);
        }
    }


}
