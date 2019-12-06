using System;
using System.IO;
using System.Linq;
using System.Configuration;
using Plch.OsTicket.Models;
using Plch.OsTicket.Models.notifications;
using Plch.OsTicket.Models.services;

namespace Plch.OsTicket.Notifications.aggregator
{
    abstract class BaseAggregator : IAggregator
    {
        protected string _connection;
        protected string _jsonStream;
        protected string _subjectKey;
        protected string _urlKey;

        public BaseAggregator(string jsonFile)
        {
            try
            {
                _connection = ConfigurationManager.ConnectionStrings["OsTicket"].ConnectionString;
                _jsonStream = File.ReadAllText($"{OsTicketEnvironment.CFG_DIR}{jsonFile}");
            }
            catch (IOException e)
            {
                throw;
            }
        }

        public void AddAdditionalRecipients(ServiceType listing, ref ViolationList list)
        {
            if (listing.AdditionalRecipients.Count > 0)
            {
                var manager = list.deptManager.ToLower();
                list.additionalRecipients = from recipients in listing.AdditionalRecipients
                                            where recipients != manager
                                            select recipients;
            }
        }

        public abstract NotificationList BuildNotificationList();

        public static class EmailKeys
        {
            public static readonly string TopicSubject = "TOPIC_SUBJECT";
            public static readonly string TaskSubject = "TASK_SUBJECT";
            public static readonly string TopicUrl = "TICKET_URL";
            public static readonly string TaskUrl = "TASK_URL";

        }
    }
}
