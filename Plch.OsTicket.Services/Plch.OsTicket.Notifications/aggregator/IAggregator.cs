using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plch.OsTicket.Models;
using Plch.OsTicket.Models.notifications;

namespace Plch.OsTicket.Notifications.aggregator
{
    interface IAggregator
    {
        NotificationList BuildNotificationList();       
    }
}
