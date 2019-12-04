using System;
using System.Configuration;

namespace Plch.Notifications.osTicket.models
{
    class Violations
    {
        public int ticketId { get; set; }
        public string ticketNum { get; set; }
        public string subject { get; set; }
        public string dueDate { get; set; }

        public string FormatTicketUrl(string urlKey)
        {
            string baseUrl = ConfigurationManager.AppSettings.Get(urlKey);
            string href = String.Format(baseUrl, ticketId);
            return $"<a href=\"{href}\">{ticketNum}</a>";
        }

    }
}
