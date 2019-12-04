using System;
using System.Data;

namespace Plch.Notifications.osTicket.data
{
    interface IConnection
    {
        string ConnectionString { get; set; }

    }
}
