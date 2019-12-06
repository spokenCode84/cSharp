using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plch.OsTicket.Notifications.aggregator
{
    class AggregatorQueue <TAggreator>
    {
        private List<TAggreator> _queue { get; set; }

        public AggregatorQueue(List<TAggreator> queue)
        {
            _queue = queue;
        }

        public List<TAggreator> GetQueue()
        {
            return _queue;
        }

        public void Add<TAggregator>(TAggreator aggregator)
        {
            try
            {
                _queue.Add(aggregator);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}
