using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Helper.Web_API_Helper_Code.Logging.Models
{
	public class SelectedItems
	{
		public List<SelectedItemList> levels { get; set; }
		public List<SelectedItemList> users { get; set; }
		public List<SelectedItemList> sources { get; set; }
	}
}
