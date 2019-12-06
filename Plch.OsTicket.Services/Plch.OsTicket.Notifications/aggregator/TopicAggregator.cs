using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Plch.OsTicket.Data;
using Plch.OsTicket.Models.services;
using Plch.OsTicket.Models.notifications;

namespace Plch.OsTicket.Notifications.aggregator
{
    class TopicAggregator : BaseAggregator
    {
        Topics _topicModel;

        public TopicAggregator(string jsonFile) : base(jsonFile)
        {
            try
            {
                _subjectKey = EmailKeys.TopicSubject;
                _urlKey = EmailKeys.TopicUrl;
                _topicModel = JsonConvert.DeserializeObject<Topics>(_jsonStream);
            }
            catch (JsonException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public override NotificationList BuildNotificationList()
        {
            NotificationDao ticketDao = new NotificationDao(_connection);
            NotificationList topicList = new NotificationList();

            foreach (Topic topic in _topicModel.HelpTopics)
            {
                ViolationList list = new ViolationList(topic.Name, _subjectKey, _urlKey);
                ticketDao.GetHelpTopicDeptManager(topic.Id, ref list);
                ticketDao.GetSlaViolations(topic.PastDueDays, topic.Id, ref list);
                ticketDao.GetCustomDateViolations(topic.PastDueDays, topic.Id, topic.Form.DueDateField, ref list);
                AddAdditionalRecipients(topic, ref list);
                topicList.violations.Add(list);
            }

            return topicList;
        }
    }
}
