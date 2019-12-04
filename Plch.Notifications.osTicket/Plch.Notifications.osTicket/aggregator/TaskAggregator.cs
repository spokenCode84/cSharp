using System;
using Plch.Notifications.osTicket.data;
using Plch.Notifications.osTicket.models;

using Newtonsoft.Json;

namespace Plch.Notifications.osTicket.aggregator
{
    class TaskAggregator: BaseAggregator
    {
        Departments _departmentModel;
        public TaskAggregator(string jsonFile) : base(jsonFile)
        {
            try
            {
                _subjectKey = EmailKeys.TaskSubject;
                _urlKey = EmailKeys.TaskUrl;
                _departmentModel = JsonConvert.DeserializeObject<Departments>(_jsonStream);
            }
            catch (JsonException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public override NotificationList BuildNotificationList()
        {
            //throw new NotImplementedException();
            OsTicketDao ticketDao = new OsTicketDao();
            NotificationList notificationList = new NotificationList();

            foreach (Department department in _departmentModel.departments)
            {
                ViolationList violationList = new ViolationList(department.Name, _subjectKey, _urlKey);
                ticketDao.GetDepartmentManager(department.Id, ref violationList);
                ticketDao.GetOverDueTaskViolations(Convert.ToString(department.Id), 
                    Convert.ToString(department.PastDueDays), ref violationList);
                AddAdditionalRecipients(department, ref violationList);
                notificationList.violations.Add(violationList);
            }
            return notificationList;
        }
    }
}
