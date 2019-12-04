using System;
using System.Reflection;
using log4net;
using CRM.Helper.Web_API_Helper_Code.Logging.Models;

namespace CRM.Helper.Web_API_Helper_Code.Logging
{
	public partial class CrmLogger: ILogger
	{

		private ILog Log;

		public CrmLogger()
		{
			this.Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		}

		public void Info(string message)
		{
			this.Log.Info(message);
		}

		public void Info(LogRequest log)
		{
			SetCustomProperties<LogRequest>(log);
			using (log4net.NDC.Push(Guid.NewGuid().ToString()))
			{
				this.Log.Info(log.message);
			}
		}

		public void Debug(string message)
		{
			this.Log.Debug(message);
		}

		public void Debug(LogRequest log)
		{
			SetCustomProperties<LogRequest>(log);
			using (log4net.NDC.Push(Guid.NewGuid().ToString()))
			{
				this.Log.Debug(log.message);
			}
		}

		public void Error(string message)
		{
			this.Log.Error(message);
		}

		public void Error(string message, Exception ex)
		{
			this.Log.Error(message, ex);
		}

		public void Error(LogRequest log)
		{
			SetCustomProperties<LogRequest>(log);
			using (log4net.NDC.Push(Guid.NewGuid().ToString()))
			{
				this.Log.Error(log.message);
			}
		}
		
		private void SetCustomProperties<TRequest>(TRequest request)
		{
			PropertyInfo[] properties = request.GetType().GetProperties();

			foreach (PropertyInfo property in properties)
			{
				if (property.PropertyType == typeof(string) && property.GetValue(request) != null)
					MDC.Set(property.Name, property.GetValue(request) as String);					
			}
		}
	}
}
