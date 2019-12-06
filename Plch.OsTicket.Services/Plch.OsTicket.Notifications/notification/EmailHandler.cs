using System;
using System.Linq;
using System.Net.Mail;
using System.Configuration;
using System.Collections.Generic;
using Plch.OsTicket.Models;
using Plch.OsTicket.Models.notifications;


namespace Plch.OsTicket.Notifications.notification
{
    class EmailHandler : INotificationHandler
    {
        private string _body;
        private string _subject;
        private string _smtpServer;
        private string _fromAddress;
        private ViolationList _list;
        private IEnumerator<string> _recipientEnum;

        public EmailHandler(ViolationList list)
        {
            _list = list;
            _smtpServer = OsTicketEnvironment.EMAIL_SERVER_KEY;
            _fromAddress = OsTicketEnvironment.EMAIL_FROM_KEY;
            _subject = FormatSubject();
            _body = FormatBody();
            _recipientEnum = _list.additionalRecipients.GetEnumerator();
        }

        private string FormatSubject()
        {
            string subject = ConfigurationManager.AppSettings.Get(_list.subjectKey);
            return String.Format(subject, _list.helpTopic);
        }

        private void AddParties(ref MailMessage email)
        {
            email.From = new MailAddress(_fromAddress);
            email.To.Add(new MailAddress(_list.deptManager));
            while (_recipientEnum.MoveNext() && _recipientEnum.Current != null)
                email.CC.Add(new MailAddress(_recipientEnum.Current));
        }

        private string FormatBody()
        {
            string _body= String.Format(OsTicketEnvironment.EMAIL_BODY_KEY,
                "<h3>", _subject, DateTime.Now.ToString("MM/dd/yyyy"), "</h3>");

            _body = _list.violations.Aggregate(_body, (body, v) =>
                    body + $"<p>{v.FormatTicketUrl(_list.urlKey)} $$ Due:{v.dueDate.Split(' ')[0]}</p>");

            return _body.Replace("$", "&nbsp;"); 
        }

        public void SendNotifications()
        {
            if (_list.violations.Count > 0)
            {
                using (SmtpClient client = new SmtpClient(_smtpServer))
                {
                    MailMessage email = new MailMessage();
                    AddParties(ref email);
                    email.Subject = _subject;
                    email.Body = _body;
                    email.IsBodyHtml = true;
                    client.UseDefaultCredentials = true;

                    try
                    {
                        client.Send(email);
                    }
                    catch (SmtpFailedRecipientsException ex)
                    {
                        Console.WriteLine(ex.Message);
                        throw;
                    }
                }
            }
            else
            {

            }
        }
    }
}
