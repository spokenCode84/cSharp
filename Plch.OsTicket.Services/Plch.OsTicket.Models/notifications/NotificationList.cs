using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plch.OsTicket.Models.notifications
{
    public class NotificationList
    {
        public List<ViolationList> violations { get; set; }

        public NotificationList()
        {
            violations = new List<ViolationList>();
        }
    }
}
