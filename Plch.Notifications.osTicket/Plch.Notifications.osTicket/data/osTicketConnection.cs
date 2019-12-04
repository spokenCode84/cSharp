using System;
using System.Data;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace Plch.Notifications.osTicket.data
{
    class osTicketConnection : IConnection
    {
        public string ConnectionString { get; set; }
        protected MySqlCommand _cmd { get; set; }
        protected MySqlDataReader _data { get; set; }
        protected MySqlConnection _connection { get; set; }
        public osTicketConnection()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["osTicket"].ConnectionString;
            _connection = new MySqlConnection(ConnectionString);
        }

        protected void OpenAndExecute(string query)
        {
            try
            {
                if (_connection.State == ConnectionState.Closed)
                    _connection.Open();

                _data = new MySqlCommand(query, _connection).ExecuteReader();
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        protected void Close()
        {
            _connection.Close();
            _data.Close();
        }
    }
}
