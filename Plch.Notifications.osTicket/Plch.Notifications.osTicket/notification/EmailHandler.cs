using System;
using System.Configuration;
using System.Net.Mail;

using Plch.Notifications.osTicket.models;
using System.Collections.Generic;

namespace Plch.Notifications.osTicket.notification
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
            _smtpServer = Env.EMAIL_SERVER_KEY;
            _fromAddress = Env.EMAIL_FROM_KEY;
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
            _body = String.Format(Env.EMAIL_BODY_KEY, 
                "<h3>", _subject, DateTime.Now.ToString("MM/dd/yyyy"), "</h3>");

            foreach (var violation in _list.violations)
            {
                string date = violation.dueDate.Split(' ')[0];
                _body += $"<p>{violation.FormatTicketUrl(_list.urlKey)} $$ Due:{date}</p>";
                _body = _body.Replace("$", "&nbsp;");
            }
            return _body;
        }

        public void SendNotifications()
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
                    if (_list.violations.Count > 0)
                    {
                        client.Send(email);
                    }
                }
                catch (SmtpFailedRecipientsException ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }             
            }
        }
    }
}
