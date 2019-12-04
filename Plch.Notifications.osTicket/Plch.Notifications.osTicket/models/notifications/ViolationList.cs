using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plch.Notifications.osTicket.models
{
    class ViolationList
    {
        public string subjectKey { get; set; }
        public string urlKey { get; set; }
        public string helpTopic { get; set; }
        public string deptManager { get; set; }

        public IEnumerable<string> additionalRecipients { get; set; }
        public List<Violations> violations { get; set; }

        public ViolationList(string helpTopic, string subjectKey, string urlKey)
        {
            this.subjectKey = subjectKey;
            this.urlKey = urlKey;
            this.helpTopic = helpTopic;
            violations = new List<Violations>();
            additionalRecipients = new List<string>();
        }
    }
}
