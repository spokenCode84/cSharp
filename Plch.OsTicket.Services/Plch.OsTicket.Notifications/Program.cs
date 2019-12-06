using System;
using System.Collections.Generic;
using static System.Environment;
using System.Linq;
using Plch.OsTicket.Models;
using Plch.OsTicket.Models.notifications;
using Plch.OsTicket.Notifications.aggregator;
using Plch.OsTicket.Notifications.notification;

namespace Plch.OsTicket.Notifications
{
    class Program
    {
        static void Main(string[] args)
        {
            AggregatorQueue<IAggregator> queue = new AggregatorQueue<IAggregator>(new List<IAggregator>());
            try
            {
                 queue.Add<TopicAggregator>(new TopicAggregator(OsTicketEnvironment.TOPIC_JSON_FILE));
                 queue.Add<TaskAggregator>(new TaskAggregator(OsTicketEnvironment.TASK_JSON_FILE));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Exit(0);
            }

            foreach (IAggregator aggregator in queue.GetQueue())
            {

                foreach (ViolationList violation in aggregator.BuildNotificationList().violations)
                {
                    INotificationHandler emailHandler = new EmailHandler(violation);
                    emailHandler.SendNotifications();
                }
            }

            Console.ReadLine();
        }
    }
}
