using MySqlConnector;
using System.Configuration;
using System.Diagnostics;

namespace GFMSLibrary.Config
{
    internal class ConnectionConfiguration
    {
        private readonly string? Server = "localhost";
        private readonly string? Username = "root";
        private readonly string? Password = "";
        private readonly string? Database = "school_db";
        private readonly string? ConnectionString;

        private MySqlConnection? _connection;

        internal MySqlConnection? Connection
        {
            get
            {
                return _connection;
            }
            set
            {
                if (_connection == value || _connection != null || value == null)
                {
                    return;
                }
                _connection = value;
            }
        }

        public ConnectionConfiguration()
        {
            ConnectionString = $"Server={Server};Database={Database};Username={Username};Password={Password}";

            try
            {
                Connection = new MySqlConnection(ConnectionString);
                Connection.OpenAsync().Wait(TimeSpan.FromSeconds(30));

                Debug.WriteLine("Connection Successful");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

    }
}