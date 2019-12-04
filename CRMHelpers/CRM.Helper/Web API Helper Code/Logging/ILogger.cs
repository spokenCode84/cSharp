using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.Helper.Web_API_Helper_Code.Logging.Models;

namespace CRM.Helper.Web_API_Helper_Code.Logging
{
	interface ILogger
	{
		void Info(string message);
		void Info(LogRequest log);
		void Debug(string message);
		void Error(string message);
		void Error(string message, Exception ex);

	}
}
