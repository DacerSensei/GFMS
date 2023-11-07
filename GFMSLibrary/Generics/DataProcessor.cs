using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Transactions;
using GFMSLibrary.Config;
using GFMSLibrary.Static;
using MySqlConnector;
using Newtonsoft.Json;

namespace GFMSLibrary.Generics
{
    internal class DataProcessor
    {
        public async Task<List<Data>> GetDataQueryAsync<Data>(string query) where Data : class, new()
        {
            List<Data> list = new List<Data>();
            Data data;

            ConnectionConfiguration configuration = new ConnectionConfiguration();
            using (MySqlCommand command = new MySqlCommand(query, configuration.Connection))
            {
                MySqlDataReader reader = await command.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    PropertyInfo[] dataProperties = typeof(Data).GetProperties().OrderBy(property => property.Name).ToArray();
                    while (await reader.ReadAsync())
                    {
                        data = new Data();

                        for (int i = 0; i < dataProperties.Length; i++)
                        {
                            int propertyIndex = Helpers.BinarySearch(dataProperties, reader.GetName(i));

                            if (propertyIndex != -1)
                            {
                                object dbValue = reader.GetValue(i);

                                if (dataProperties[propertyIndex].PropertyType == typeof(int?))
                                {
                                    dataProperties[propertyIndex].SetValue(data, (int)dbValue);
                                }
                                else if (dbValue.GetType() == typeof(byte[]))
                                {
                                    dataProperties[propertyIndex].SetValue(data, Encoding.UTF8.GetString((byte[])dbValue));
                                }
                                else
                                {
                                    dataProperties[propertyIndex].SetValue(data, Convert.ChangeType(dbValue, dataProperties[propertyIndex].PropertyType));
                                }
                            }
                        }

                        list.Add(data);
                    }
                }
                else
                {
                    Debug.WriteLine("No Rows");
                }
                await command.Connection!.DisposeAsync();
            }
            return list;
        }

        public async Task<List<Data>> GetDataQueryAsync<Data>(MySqlCommand MysqlCommand) where Data : class, new()
        {
            List<Data> list = new List<Data>();
            Data data;

            using (MySqlCommand command = MysqlCommand)
            {
                MySqlDataReader reader = await command.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    PropertyInfo[] dataProperties = typeof(Data).GetProperties().OrderBy(property => property.Name).ToArray();
                    while (await reader.ReadAsync())
                    {
                        data = new Data();

                        for (int i = 0; i < dataProperties.Length; i++)
                        {
                            int propertyIndex = Helpers.BinarySearch(dataProperties, reader.GetName(i));

                            if (propertyIndex != -1)
                            {
                                object dbValue = reader.GetValue(i);
                                Console.WriteLine(dataProperties[propertyIndex].PropertyType);
                                Console.WriteLine(dbValue);

                                if (dataProperties[propertyIndex].PropertyType == typeof(int?))
                                {
                                    dataProperties[propertyIndex].SetValue(data, (int)dbValue);
                                }
                                else if (dbValue.GetType() == typeof(byte[]))
                                {
                                    dataProperties[propertyIndex].SetValue(data, Encoding.UTF8.GetString((byte[])dbValue));
                                }
                                else
                                {
                                    dataProperties[propertyIndex].SetValue(data, Convert.ChangeType(dbValue, dataProperties[propertyIndex].PropertyType));
                                }
                            }
                        }

                        list.Add(data);
                    }
                }
                else
                {
                    Debug.WriteLine("No Rows");
                }
                await command.Connection!.DisposeAsync();
            }
            return list;
        }

        private long _lastInsertedId;
        internal long LastInsertedId
        {
            get
            {
                long value = _lastInsertedId;
                _lastInsertedId = 0;
                return value;
            }
            private set
            {
                if (_lastInsertedId != value)
                {
                    _lastInsertedId = value;
                }
            }
        }

        public async Task<bool> CreateDataQueryAsync<Data>(Data data, string tableName) where Data : class
        {
            StringBuilder query = new StringBuilder();
            ConnectionConfiguration configuration = new ConnectionConfiguration();
            query.Append($"INSERT INTO {tableName} SET ");
            try
            {
                List<PropertyInfo> dataProperties = typeof(Data).GetProperties().OrderBy(property => property.Name).ToList();

                int IndexID = Helpers.BinarySearch(dataProperties, "id");
                if (IndexID != -1)
                {
                    dataProperties.RemoveAt(IndexID);
                }
                List<string> setClauses = dataProperties.Select(property => $"{property.Name.ToLower()} = @{property.Name.ToLower()}").ToList();
                query.Append(string.Join(", ", setClauses));
                using (MySqlCommand command = new MySqlCommand(query.ToString(), configuration.Connection))
                {
                    for (int i = 0; i < dataProperties.Count; i++)
                    {
                        command.Parameters.AddWithValue($"@{dataProperties[i].Name.ToLower()}", dataProperties[i].GetValue(data)?.ToString());
                    }
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    LastInsertedId = command.LastInsertedId;
                    await command.Connection!.DisposeAsync();
                    if (rowsAffected > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> UpdateDataQueryAsync<Data, Where>(Data data, Where wheres, string tableName) where Data : class
        {
            StringBuilder query = new StringBuilder();
            ConnectionConfiguration configuration = new ConnectionConfiguration();
            query.Append($"UPDATE {tableName} SET ");
            try
            {
                List<PropertyInfo> dataProperties = typeof(Data).GetProperties()
                    .Where(property => property.GetValue(data) != null)
                    .OrderBy(property => property.Name).ToList();
                PropertyInfo[] whereProperties = typeof(Where).GetProperties();

                int IndexID = Helpers.BinarySearch(dataProperties, "id");
                if (IndexID != -1)
                {
                    dataProperties.RemoveAt(IndexID);
                }

                List<string> setClauses = dataProperties.Select(property => $"{property.Name.ToLower()} = @{property.Name.ToLower()}").ToList();
                query.Append(string.Join(", ", setClauses));
                query.Append(" WHERE ");
                List<string> whereClauses = whereProperties
                    .Select(property => $"{property.Name.ToLower()} = @{property.Name.ToLower()}")
                    .ToList();
                query.Append(string.Join(" AND ", whereClauses));
                using (MySqlCommand command = new MySqlCommand(query.ToString(), configuration.Connection))
                {
                    for (int i = 0; i < dataProperties.Count; i++)
                    {
                        command.Parameters.AddWithValue($"@{dataProperties[i].Name.ToLower()}", dataProperties[i].GetValue(data)?.ToString());
                    }
                    for (int i = 0; i < whereProperties.Length; i++)
                    {
                        command.Parameters.AddWithValue($"@{whereProperties[i].Name.ToLower()}", whereProperties[i].GetValue(wheres)?.ToString());
                    }
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    await command.Connection!.DisposeAsync();
                    if (rowsAffected > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteDataQueryAsync<Where>(Where wheres, string tableName) where Where : class
        {
            StringBuilder query = new StringBuilder();
            ConnectionConfiguration configuration = new ConnectionConfiguration();
            query.Append($"DELETE FROM {tableName} WHERE ");
            try
            {
                PropertyInfo[] whereProperties = typeof(Where).GetProperties();

                List<string> whereClauses = whereProperties
                    .Select(property => $"{property.Name.ToLower()} = @{property.Name.ToLower()}")
                    .ToList();
                query.Append(string.Join(" AND ", whereClauses));
                using (MySqlCommand command = new MySqlCommand(query.ToString(), configuration.Connection))
                {
                    for (int i = 0; i < whereProperties.Length; i++)
                    {
                        command.Parameters.AddWithValue($"@{whereProperties[i].Name.ToLower()}", whereProperties[i].GetValue(wheres)?.ToString());
                    }
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    await command.Connection!.DisposeAsync();
                    if (rowsAffected > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
