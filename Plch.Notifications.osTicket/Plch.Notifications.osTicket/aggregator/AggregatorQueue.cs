using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plch.Notifications.osTicket.models;

namespace Plch.Notifications.osTicket.aggregator
{
    class AggregatorQueue
    {
        private List<IAggregator> _queue { get; set; }

        public AggregatorQueue(List<IAggregator> queue)
        {
            _queue = queue;
        }

        public List<IAggregator> GetQueue()
        {
            return _queue;
        }

        public void Add(IAggregator aggregator)
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
