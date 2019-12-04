using System;
using System.IO;
using CRM.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Configuration;

namespace CRM.Tests
{
    [TestClass]
    public class CRMTesting
    {
        [TestMethod]
        public void TestCRMConnection()
        {
            CRMConnector.CRMConnectorTest();
        }


        [TestMethod]
        public void GetAPIVersion()
        {

            APIOperations operations = new APIOperations();

            try
            {
                string configpath = Environment.CurrentDirectory + "\\app.config";

                //string appconfigPath = Direc
                operations.ConnectToCRM(new string[1]{ configpath });

                Task.WaitAll(Task.Run(async () => await operations.RunAsync()));

                //operations.getWebAPIVersion().RunSynchronously();
            }
            catch
            {
                throw;
            }
        }


        [TestMethod]
        public void GetAPIVersionWithConnectionString()
        {
            APIOperations operations = new APIOperations();

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;

                //string appconfigPath = Direc
                operations.ConnectToCRMWithConnectionString(connectionString);

                Task.WaitAll(Task.Run(async () => await operations.RunAsync()));

                //operations.getWebAPIVersion().RunSynchronously();
            }
            catch
            {
                throw;
            }
        }
    }
}
