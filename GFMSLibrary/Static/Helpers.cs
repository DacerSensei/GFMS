using GFMSLibrary.Config;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GFMSLibrary.Static
{
    internal static class Helpers
    {
        internal static MySqlCommand CreateMysqlCommand(string query)
        {
            ConnectionConfiguration config = new ConnectionConfiguration();
            return new MySqlCommand(query, config.Connection);
        }

        internal static int BinarySearch(List<string> list, string targetName)
        {
            int left = 0;
            int right = list.Count - 1;

            while (left <= right)
            {
                int middle = (left + right) / 2;

                int comparison = string.Compare(list[middle], targetName);

                if (comparison == 0)
                    return middle;
                else if (comparison > 0)
                    right = middle - 1; // Search in the left half
                else
                    left = middle + 1; // Search in the right half
            }

            return -1; // Target not found in the sorted list
        }

        internal static int BinarySearch(PropertyInfo[] list, string targetName)
        {
            int left = 0;
            int right = list.Length - 1;

            while (left <= right)
            {
                int middle = (left + right) / 2;

                int comparison = string.Compare(list[middle].Name, targetName, StringComparison.OrdinalIgnoreCase);

                if (comparison == 0)
                    return middle;
                else if (comparison > 0)
                    right = middle - 1; // Search in the left half
                else
                    left = middle + 1; // Search in the right half
            }

            return -1; // Target not found in the sorted list
        }

        internal static int BinarySearch(List<PropertyInfo> list, string targetName)
        {
            int left = 0;
            int right = list.Count - 1;

            while (left <= right)
            {
                int middle = (left + right) / 2;

                int comparison = string.Compare(list[middle].Name, targetName, StringComparison.OrdinalIgnoreCase);

                if (comparison == 0)
                    return middle;
                else if (comparison > 0)
                    right = middle - 1; // Search in the left half
                else
                    left = middle + 1; // Search in the right half
            }

            return -1; // Target not found in the sorted list
        }
    }
}
