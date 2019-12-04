using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFB.CRM.Services.Lead.handlers
{
	public interface IHandler
	{
		void ProcessMessage(String body);
	}
}
