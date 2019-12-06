using System;
using System.IO;
using System.Configuration;
using System.Collections.Generic;


namespace Plch.OsTicket.Models
{
    public class OsTicketEnvironment
    {
        public static String BASE_DIR = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "..", "..");
        public static String CFG_DIR = BASE_DIR + @"\config\";
        public static String TASK_JSON_FILE = ConfigurationManager.AppSettings.Get("TASK_JSON");
        public static String TOPIC_JSON_FILE = ConfigurationManager.AppSettings.Get("TOPIC_JSON");
        public static String EMAIL_BODY_KEY = ConfigurationManager.AppSettings.Get("TOPIC_BODY");
        public static String EMAIL_SERVER_KEY = ConfigurationManager.AppSettings.Get("SMTP_SERVER");
        public static String EMAIL_FROM_KEY = ConfigurationManager.AppSettings.Get("FROM_ADDRESS");
    }
}
