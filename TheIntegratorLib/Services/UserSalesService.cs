using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TheIntegratorLib.Models;
using TheIntegratorLib.Utilities;

namespace TheIntegratorLib.Services
{
    public interface IUserSalesService
    {
        void UseCache(IDataCache userSalesCache);
        void Record(string row);
        void SetHeader(string record);
        List<UserSalesModel> GetSales(DateTime? fromDate, DateTime? toDate);
    }
    public class UserSalesService : IUserSalesService
    {
        private readonly ICSVReader _csvReader;
        private IDataCache _userSalesCache;

        public UserSalesService(ICSVReader csvReader)
        {
            _csvReader = csvReader;
        }

        public void UseCache(IDataCache userSalesCache)
        {
            _userSalesCache = userSalesCache;
            Dictionary<int, string> headers = _userSalesCache.Get<Dictionary<int, string>>(DataCacheKey.Headers);
            if (headers.Count > 0)
                _csvReader.SetHeader(headers);
        }

        public List<UserSalesModel> GetSales(DateTime? fromDate, DateTime? toDate)
        {
            List<UserSalesModel> userSales = _userSalesCache.Get<List<UserSalesModel>>(DataCacheKey.UserSales);
            if (fromDate != null)
                userSales = userSales.Where(s => s.Last_Purchase_Date >= fromDate).ToList();
            if (toDate != null)
                userSales = userSales.Where(s => s.Last_Purchase_Date <= toDate).ToList();
            return userSales;
        }

        public void SetHeader(string record)
        {
            if (!_csvReader.HasHeader())
            {
                IEnumerable<string> strHeaders = _csvReader.GetHeader(record);
                _csvReader.SetHeader(strHeaders);
                _userSalesCache.Create(_csvReader.GetHeader(), DataCacheKey.Headers, DataCacheDuration.Medium);
            } else
            {
                Dictionary<int, string> headers = _userSalesCache.Get<Dictionary<int,string>>(DataCacheKey.Headers);
                _csvReader.SetHeader(headers);
            }     
        }

        public void Record(string row)
        {
            List<UserSalesModel> useSalesCache = _userSalesCache.Get<List<UserSalesModel>>(DataCacheKey.UserSales);
            UserSalesModel model = _csvReader.ReadRow<UserSalesModel>(row);
            IList<ValidationResult> validateResult = ValidateModel(model);
            if (validateResult.Count == 0)
            {
                useSalesCache.Add(model);
                _userSalesCache.Create(useSalesCache, DataCacheKey.UserSales, DataCacheDuration.Medium);
            }
        }

        private IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }
    }
}
