using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Plch.OsTicket.Data
{
    public class OsTicketConnection : IConnection
    {
        public string ConnectionString { get; set; }
        protected MySqlCommand _cmd { get; set; }
        protected MySqlDataReader _data { get; set; }
        protected MySqlConnection _connection { get; set; }
        public OsTicketConnection(string connection)
        {
            _connection = new MySqlConnection(connection);
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
