using System;
using System.Configuration;
using MySql.Data.MySqlClient;
using static System.Environment;
using System.Collections.Generic;
using Plch.Notifications.osTicket.models;
using Plch.Notifications.osTicket.aggregator;
using Plch.Notifications.osTicket.notification;


namespace Plch.Notifications.osTicket
{
    class Program
    {
        static void Main(string[] args)
        {
            AggregatorQueue queue = new AggregatorQueue(new List<IAggregator>());

            try
            {
                queue.Add(new TopicAggregator(Env.TOPIC_JSON_FILE));
                queue.Add(new TaskAggregator(Env.TASK_JSON_FILE));
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
        }
    }
}
