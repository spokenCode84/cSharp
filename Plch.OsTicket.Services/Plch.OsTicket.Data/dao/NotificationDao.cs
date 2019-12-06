using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plch.OsTicket.Models.notifications;

namespace Plch.OsTicket.Data
{
    public class NotificationDao : OsTicketConnection
    {
        public NotificationDao(string connection) : base(connection) { }

        public void GetSlaViolations(int daysOld, int topicId, ref ViolationList list)
        {
            OpenAndExecute(String.Format(OsTicketQueries.GetSlaViolations, daysOld, topicId));
            AddViolationFromReader(ref list);
            Close();
        }

        public void GetCustomDateViolations(int daysOld, int topic, string entryId, ref ViolationList list)
        {
            OpenAndExecute(String.Format(OsTicketQueries.GetCustomDateViolations, entryId, topic, daysOld));
            AddViolationFromReader(ref list);
            Close();
        }

        public void GetOverDueTaskViolations(string deptId, string daysOld, ref ViolationList list)
        {
            OpenAndExecute(String.Format(OsTicketQueries.GetOverDueTasks, deptId, daysOld));
            AddViolationFromReader(ref list);
            Close();
        }

        public void GetHelpTopicDeptManager(int topicId, ref ViolationList list)
        {
            OpenAndExecute(String.Format(OsTicketQueries.GetHelpTopicDeptManager, topicId));
            AddManager(ref list);
            Close();
        }

        public void GetDepartmentManager(int deptId, ref ViolationList list)
        {
            OpenAndExecute(String.Format(OsTicketQueries.GetDeptManager, deptId));
            AddManager(ref list);
            Close();
        }

        public void AddManager(ref ViolationList list)
        {
            if (_data.Read())
                list.deptManager = _data.GetString(0);
        }

        public void AddViolationFromReader(ref ViolationList list)
        {
            while (_data.Read())
            {
                list.violations.Add(
                    new Violations
                    {
                        ticketId = _data.GetInt32(0),
                        ticketNum = _data.GetString(1),
                        subject = _data.GetString(2),
                        dueDate = _data.GetString(3)
                    }
                );
            }
        }
    } // END: Class

    static class OsTicketQueries
    {
        public static string GetSlaViolations =
        @"SELECT t.ticket_id, t.number, td.subject, t.est_duedate
        FROM ost_ticket t
        INNER JOIN ost_ticket__cdata td on t.ticket_id = td.ticket_id
        WHERE t.status_id = 1
        AND est_duedate >= DATE_ADD(CURDATE(), INTERVAL -{0} DAY)
        AND t.topic_id = {1}
        ORDER BY est_duedate ASC;";

        public static string GetCustomDateViolations =
        @"SELECT t.ticket_id, t.number, td.subject, ev.value AS due_date
        FROM ost_ticket t
        INNER JOIN ost_ticket__cdata td on t.ticket_id = td.ticket_id
        INNER JOIN ost_form_entry fe ON t.ticket_id = fe.object_id
        INNER JOIN ost_form_entry_values ev ON fe.id = ev.entry_id
        WHERE t.status_id = 1
        AND ev.field_id = {0}
        AND t.topic_id = {1}
        AND ev.value BETWEEN DATE(NOW()) - INTERVAL {2} DAY AND DATE(NOW())
        ORDER BY ev.value ASC;";

        public static string GetOverDueTasks =
        @"SELECT t.number, t.object_id, td.title, t.duedate 
        FROM ost_task t
        INNER JOIN ost_task__cdata td ON t.id = td.task_id
        WHERE t.closed is null
        AND t.dept_id = {0}
        AND t.duedate BETWEEN DATE(NOW()) - INTERVAL {1} DAY AND DATE(NOW());";

        public static string GetDeptManager =
        @"SELECT s.email
        FROM ost_department d
        INNER JOIN ost_staff s ON d.manager_id = s.staff_id
        WHERE d.id = {0};";

        public static string GetHelpTopicDeptManager =
        @"SELECT s.email FROM ost_help_topic ht
        INNER JOIN ost_department d on ht.dept_id = d.id
        INNER JOIN ost_staff s on d.manager_id = s.staff_id
        WHERE ht.topic_id = {0};";
    }
}

