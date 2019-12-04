using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Data.SqlClient;
using CRM.Helper.Web_API_Helper_Code.Logging.Appenders;

namespace WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Enable CORS
            // https://docs.microsoft.com/en-us/aspnet/web-api/overview/security/enabling-cross-origin-requests-in-web-api
            // go back and re-visit global
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

			try
			{
				CrmAdoAppender.Create(AppenderConfig.ToXmlString());
			}
			catch (SqlException e)
			{
				CrmFileAppender.Create(FileAppenderConfig.ToXmlString());
			}

        }


    }
}
