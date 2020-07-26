using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TheIntegratorLib.Models;
using TheIntegratorLib.Services;
using TheIntegratorLib.Utilities;

namespace TheIntegratorLibTests
{
    [TestClass]
    public class UserSalesServiceTests
    {       
        [DataTestMethod]
        [DataRow("user_name,age,height,gender,sale_amount,last_purchase_date",
                 "John Doe, 29, 177, M, 21312, 2020-11-05T13:15:30Z")]
        public void UserSalesService_SetHeader_Record_GetSales(string row, string record)
        {
            ICSVReader csvReader = new CSVReader();
            MemoryCacheOptions options = new MemoryCacheOptions();
            IMemoryCache cache = new MemoryCache(options);
            IDataCache dataCache = new UserSalesCache();
            dataCache.SetCache(cache);
            IUserSalesService userSalesService = new UserSalesService(csvReader);
            userSalesService.UseCache(dataCache);

            userSalesService.SetHeader(row);
            userSalesService.Record(record);
            List<UserSalesModel> userSales = userSalesService.GetSales(null, null);

            Assert.AreEqual("John Doe", userSales[0].User_Name);
        }
    }

}