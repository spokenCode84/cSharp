using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plch.OsTicket.Data
{
    interface IConnection
    {
        string ConnectionString { get; set; }
    }
}
