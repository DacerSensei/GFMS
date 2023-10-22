using GFMSLibrary.Generics;
using GFMSLibrary.Static;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFMSLibrary
{
    public class LoginCredentials
    {
        public List<T> Login<T>(string username, string password, string tableName) where T : class, new()
        {
            DataProcessor processor = new DataProcessor();
            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM ");
            query.Append(tableName);
            query.Append(" WHERE username = @username AND password = @password");
            MySqlCommand command = Helpers.CreateMysqlCommand(query.ToString());
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", password);
            return processor.GetDataQueryAsync<T>(command).Result;
        }

        public bool Register<T>(T data, string tableName) where T : class, new()
        {
            DataProcessor processor = new DataProcessor();
            return processor.CreateDataQueryAsync(data, tableName).Result;
        }
    }
}
