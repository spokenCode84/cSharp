using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Helper.Web_API_Helper_Code.Logging.Exceptions
{
	public class LoggingException : Exception
	{
		public LoggingException() { }

		public LoggingException(string message) : base(message) { }
	}
}
