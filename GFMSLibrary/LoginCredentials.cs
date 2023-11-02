using GFMSLibrary.Generics;
using GFMSLibrary.Static;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace GFMSLibrary
{
    public class LoginCredentials
    {
        private DataProcessor processor;
        public LoginCredentials()
        {
            processor = new DataProcessor();

        }
        public async Task<T> Login<T>(string username, string password, string tableName) where T : class, new()
        {
            DataProcessor processor = new DataProcessor();
            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM ");
            query.Append(tableName);
            query.Append(" WHERE username = @username AND password = @password");
            MySqlCommand command = Helpers.CreateMysqlCommand(query.ToString());
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", password);
            List<T> user = await processor.GetDataQueryAsync<T>(command);
            return user.Count > 0 && user[0] != null ? user[0] : null! ;
        }

        public async Task<T> GetByIdAsync<T>(string id, string tableName) where T : class, new()
        {
            DataProcessor processor = new DataProcessor();
            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM ");
            query.Append(tableName);
            query.Append(" WHERE id = @id");
            MySqlCommand command = Helpers.CreateMysqlCommand(query.ToString());
            command.Parameters.AddWithValue("@id", id);
            List<T> user = await processor.GetDataQueryAsync<T>(command);
            return user.Count > 0 && user[0] != null ? user[0] : null!;
        }

        public bool Register<T>(T data, string tableName) where T : class, new()
        {
            DataProcessor processor = new DataProcessor();
            return processor.CreateDataQueryAsync(data, tableName).Result;
        }

        public bool RegisterStudent<T>(T data, string tableName) where T : class, new()
        {
            DataProcessor processor = new DataProcessor();
            return processor.CreateDataQueryAsync(data, tableName).Result;
        }

        public async Task<bool> RegisterStudentAsync<T>(T data, string tableName) where T : class, new()
        {
            return await processor.CreateDataQueryAsync(data, tableName);
        }

        public async Task<bool> UpdateStudentAsync<TData, TWhere>(TData data, TWhere wheres , string tableName) where TData : class, new()
        {
            return await processor.UpdateDataQueryAsync(data, wheres, tableName);
        }

        public long GetLastInsertedId()
        {
            return processor.LastInsertedId;
        }

        public async Task<List<T>> GetAllDataAsync<T>(string tableName) where T : class, new()
        {
            DataProcessor processor = new DataProcessor();
            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM ");
            query.Append(tableName);
            MySqlCommand command = Helpers.CreateMysqlCommand(query.ToString());
            return await processor.GetDataQueryAsync<T>(command);
        }

        public async Task<List<T>> GetAllDataAsync<T, Where>(string tableName, Where wheres) where T : class, new()
        {
            PropertyInfo[] whereProperties = typeof(Where).GetProperties();
            List<string> whereClauses = whereProperties
                    .Select(property => $"{property.Name.ToLower()} = @{property.Name.ToLower()}")
                    .ToList();
            DataProcessor processor = new DataProcessor();
            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM ");
            query.Append(tableName);
            query.Append(" WHERE ");
            query.Append(string.Join(" AND ", whereClauses));
            Debug.WriteLine(query.ToString());
            MySqlCommand command = Helpers.CreateMysqlCommand(query.ToString());
            for (int i = 0; i < whereProperties.Length; i++)
            {
                command.Parameters.AddWithValue($"@{whereProperties[i].Name.ToLower()}", whereProperties[i].GetValue(wheres)?.ToString());
            }
            return await processor.GetDataQueryAsync<T>(command);
        }

        public async Task<bool> DeleteDataAsync<TWhere>(string tableName, TWhere where) where TWhere : class
        {
            return await processor.DeleteDataQueryAsync(where, tableName);
        }
    }
}
