using System;
using System.IO;
using System.Text;
using System.Configuration;
using log4net.Config;

using CRM.Helper.Web_API_Helper_Code.Logging.Models;


namespace CRM.Helper.Web_API_Helper_Code.Logging.Appenders
{

	public static class CrmAdoAppender
	{

		public static void Create(string xmlConfig)
		{
			log4net.Util.LogLog.InternalDebugging = true;
			XmlConfigurator.Configure(new MemoryStream(Encoding.UTF8.GetBytes(AppenderConfig.ToXmlString())));
		}

	}

	public static class AppenderConfig
	{

		public static string ToXmlString()
		{
			CONFIG = CONFIG.Replace("{0}", ConnectionBuilder.ConnectionBuilder.GetDatabaseCxnString());
			CONFIG = CONFIG.Replace("{1}", LogProperties.User);
			CONFIG = CONFIG.Replace("{2}", LogProperties.Source);
			CONFIG = CONFIG.Replace("{3}", LogProperties.Request);
			CONFIG = CONFIG.Replace("{4}", LogProperties.Response);
			CONFIG = CONFIG.Replace("{5}", LogProperties.Stacktrace);
			return CONFIG;
		}

		private static string CONFIG =
			@"
			<log4net>
				<appender name='AdoNetAppender' type='log4net.Appender.AdoNetAppender'>
					<bufferSize value='1' />
					<connectionType value='System.Data.SqlClient.SqlConnection' />
					<connectionString value='{0}' />
					<commandText value='INSERT INTO dbo.CRM_Logs ([Date],[Thread],[Level],[Logger],[Message],[UserOpr],[Source],[Request],[Response],[Stacktrace],[Exception]) VALUES (@log_date, @thread, @log_level, @logger, @message, @user, @source, @request, @response, @stacktrace, @exception)' />
					<parameter>
						<parameterName value='@log_date' />
						<dbType value='DateTime' />
						<layout type='log4net.Layout.RawTimeStampLayout' />
					</parameter>
					<parameter>
						<parameterName value='@thread' />
						<dbType value='String' />
						<size value='255' />
						<layout type='log4net.Layout.PatternLayout' value='%thread' />
					</parameter>
					<parameter>
						<parameterName value='@log_level' />
						<dbType value='String' />
						<size value='50' />
						<layout type='log4net.Layout.PatternLayout' value='%level' />
					</parameter>
					<parameter>
						<parameterName value='@logger' />
						<dbType value='String' />
						<size value='255' />
						<layout type='log4net.Layout.PatternLayout' value='%logger' />
					</parameter>
					<parameter>
						<parameterName value='@message' />
						<dbType value='String' />
						<size value='-1' />
						<layout type='log4net.Layout.PatternLayout' value='%message' />
					</parameter>
					<parameter>
						<parameterName value='@user' />
						<dbType value='String' />
						<size value='255' />
						<layout type='log4net.Layout.PatternLayout'>
							<conversionPattern value='%property{{1}}' />
						</layout>
					</parameter>
					<parameter>
						<parameterName value='@source' />
						<dbType value='String' />
						<size value='255' />
						<layout type='log4net.Layout.PatternLayout'>
							<conversionPattern value='%property{{2}}' />
						</layout>
					</parameter>
					<parameter>
						<parameterName value='@request' />
						<dbType value='String' />
						<size value='8000' />
						<layout type='log4net.Layout.PatternLayout'>
							<conversionPattern value='%property{{3}}' />
						</layout>
					</parameter>
					<parameter>
						<parameterName value='@response' />
						<dbType value='String' />
						<size value='8000' />
						<layout type='log4net.Layout.PatternLayout'>
							<conversionPattern value='%property{{4}}' />
						</layout>
					</parameter>
					<parameter>
						<parameterName value='@stacktrace' />
						<dbType value='String' />
						<size value='4000' />
						<layout type='log4net.Layout.PatternLayout'>
							<conversionPattern value='%property{{5}}' />
						</layout>
					</parameter>
					<parameter>
						<parameterName value='@exception' />
						<dbType value='String' />
						<size value='-1' />
						<layout type='log4net.Layout.ExceptionLayout' />
					</parameter>
				</appender>
				<root>
					<level value='ALL' />
					<appender-ref ref='AdoNetAppender' />
				</root>
			</log4net>";
	}
}
