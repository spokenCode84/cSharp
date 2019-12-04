using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plch.Notifications.osTicket.models;

namespace Plch.Notifications.osTicket.aggregator
{
    interface IAggregator
    {
        NotificationList BuildNotificationList();
    }
}
