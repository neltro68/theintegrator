using System;
using System.Collections.Generic;
using System.Reflection;
using TheIntegratorLib.Models;

namespace TheIntegratorLib.Utilities
{
    public interface ICSVReader
    {
        IEnumerable<string> GetHeader(string row);
        void SetHeader(Dictionary<int, string> headers);
        void SetHeader(IEnumerable<string> headers);
        bool HasHeader();
        Dictionary<int, string> GetHeader();
        T ReadRow<T>(string row) where T : new();
    }

    public class CSVReader : ICSVReader
    {
        private Dictionary<int,string> _headers = new Dictionary<int, string>();
        public CSVReader()
        {
        }

        public IEnumerable<string> GetHeader(string row)
        {
            if (string.IsNullOrEmpty(row))
                yield return null;

            string[] headers = row.Split(",");
            foreach (string header in headers)
                yield return header;

        }

        public void SetHeader(IEnumerable<string> headers)
        {
            PropertyInfo[] props = Type.GetType("TheIntegratorLib.Models.IUserSales").GetProperties();
            foreach (string header in headers)
            {
                int index = 0;
                foreach (PropertyInfo prop in props)
                {
                    if (prop.Name.ToLower() == header)
                    {
                        _headers[index] = header;
                        break;
                    }
                    index++;
                }
            }
        }

        public void SetHeader(Dictionary<int,string> headers)
        {
            _headers = headers;
        }

        public bool HasHeader()
        {
            return _headers.Count > 0;
        }

        public Dictionary<int,string> GetHeader()
        {
            return _headers;
        }

        public T ReadRow<T>(string row) where T : new()
        {
            IUserSales userSales = new T() as UserSalesModel;
           
            if (_headers.Count == 0) return (T)userSales;

            string[] cellValues = row.Split(",");
            PropertyInfo[] props = userSales.GetType().GetProperties();

            for (int i = 0; i < cellValues.Length; i++)
            {
                foreach(PropertyInfo prop in props)
                {
                    if (prop.Name.ToLower() == _headers[i])
                    {
                        if (prop.PropertyType == typeof(int))
                            prop.SetValue(userSales, Convert.ToInt32(cellValues[i]));
                        else if (prop.PropertyType == typeof(double))
                            prop.SetValue(userSales, Convert.ToDouble(cellValues[i]));
                        else if (prop.PropertyType == typeof(DateTime))
                            prop.SetValue(userSales, Convert.ToDateTime(cellValues[i]));
                        else
                            prop.SetValue(userSales, cellValues[i].Trim());

                    }
                }
            }
            return (T)userSales;
        }

    }
}
