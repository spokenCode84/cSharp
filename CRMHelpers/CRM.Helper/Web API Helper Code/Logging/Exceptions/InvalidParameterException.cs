using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Helper.Web_API_Helper_Code.Logging.Exceptions
{
	public class InvalidParameterException : LoggingException
	{
		public InvalidParameterException() { }

		public InvalidParameterException(string message) : base(message) { }
	}
}
