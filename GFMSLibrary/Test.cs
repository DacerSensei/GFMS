using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GFMSLibrary.Config;
using GFMSLibrary.Generics;
using GFMSLibrary.Static;
using MySqlConnector;

namespace GFMSLibrary
{
    public class Test
    {
        public Test()
        {
            
        }

        public void CreateTest<Data>(Data data, string tableName) where Data : class, new()
        {
            DataProcessor dataProcessor = new DataProcessor();
            if (dataProcessor.CreateDataQueryAsync(data, tableName).Result)
            {
                Debug.WriteLine("Success");
            }else
            {
                Debug.WriteLine("Not Successful");
            }
        }

        public void UpdateTest<Data, Where>(Data data, Where where, string tableName) where Data : class, new()
        {
            DataProcessor dataProcessor = new DataProcessor();
            if (dataProcessor.UpdateDataQueryAsync(data, where, tableName).Result)
            {
                Debug.WriteLine("Success");
            }
            else
            {
                Debug.WriteLine("Not Successful");
            }
        }

        public List<Data> GetTest<Data>() where Data : class, new()
        {
            DataProcessor dataProcessor = new DataProcessor();
            var list = dataProcessor.GetDataQueryAsync<Data>(Helpers.CreateMysqlCommand("SELECT * FROM users"));
            return list.Result;
        }
    }
}
