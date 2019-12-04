using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CRM.Helper
{
    public static class RetrieveMultipleHelper
    {
        public static EntityCollection RetrieveMultiple(string entityName)
        {
            var crmSvc = CRMConnector.GetCrmClient();

            EntityCollection foundEntities = crmSvc.RetrieveMultiple(new QueryExpression(entityName));

            return foundEntities;
        }
    }
}
