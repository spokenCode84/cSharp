using System;
using System.IO;
using System.Text;
using System.Configuration;
using log4net.Config;

namespace CRM.Helper.Web_API_Helper_Code.Logging.Appenders
{
	public static class CrmFileAppender
	{
		public static void Create(string xmlConfig)
		{
			log4net.Util.LogLog.InternalDebugging = true;
			XmlConfigurator.Configure(new MemoryStream(Encoding.UTF8.GetBytes(xmlConfig)));
		}

	}

	public static class FileAppenderConfig
	{
		public static string ToXmlString()
		{
			return
			@"
			<log4net>
			  <root>
				<level value='ALL' />
				<appender-ref ref='console' />
				<appender-ref ref='file' />
			  </root>
			  <appender name='console' type='log4net.Appender.ConsoleAppender'>
				<layout type='log4net.Layout.PatternLayout'>
				  <conversionPattern value='%date %level %logger - %message%newline' />
				</layout>
			  </appender>
			  <appender name='file' type='log4net.Appender.RollingFileAppender'>
				<file value='C:\Users\opr1113\Desktop\logs\web-log.txt' /> 
				<appendToFile value='true' />
				<rollingStyle value='Size' />
				<maxSizeRollBackups value='5' />
				<maximumFileSize value='10MB' />
				<staticLogFileName value='true' />
				<layout type='log4net.Layout.PatternLayout'>
				  <conversionPattern value='%date [%thread] %level %logger - %message%newline' />
				</layout>
			  </appender>
			</log4net>";
		}
	}
}
