using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IFB.CRM.Services.Lead.handlers;

namespace IFB.CRM.Services.Lead.subscriptions
{
	public interface ISubscription
	{
		string URI { get; set; }
		string ClientId { get; set; }
		string userName { get; set; }
		string password { get; set; }
		string destination { get; set; }
		IHandler handler { get; set; }
		Boolean Listen();
	}
}
