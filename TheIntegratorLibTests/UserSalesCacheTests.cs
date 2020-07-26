using System;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TheIntegratorLib.Models;
using TheIntegratorLib.Utilities;

namespace TheIntegratorLibTests
{
    [TestClass]
    public class UserSalesCacheTests
    {       
        [TestMethod]
        public void UserSalesCache_CreateThenGet_Test()
        {
            MemoryCacheOptions options = new MemoryCacheOptions();
            IMemoryCache cache = new MemoryCache(options);
            IDataCache dataCache = new UserSalesCache(cache);
            IUserSales userSales = new UserSalesModel()
            {
                User_Name = "Renel Castro",
            };

            string dateTime = DateTime.Now.ToString("YYYY-MM-dd");
            dataCache.Create(userSales, DataCacheKey.Sales, DataCacheDuration.Short, dateTime);
            UserSalesModel userSalesCache = dataCache.Get<UserSalesModel>(DataCacheKey.Sales, dateTime);

            Assert.AreEqual("Renel Castro", userSalesCache.User_Name);
        }

        [TestMethod]
        public void UserSalesCache_Remove_Test()
        {
            MemoryCacheOptions options = new MemoryCacheOptions();
            IMemoryCache cache = new MemoryCache(options);
            IDataCache dataCache = new UserSalesCache(cache);
            IUserSales userSales = new UserSalesModel()
            {
                User_Name = "Renel Castro",
            };
            string dateTime = DateTime.Now.ToString("YYYY-MM-dd");
            dataCache.Create(userSales, DataCacheKey.Sales, DataCacheDuration.Short, dateTime);
            dataCache.Remove(DataCacheKey.Sales, dateTime);
            UserSalesModel userSalesCache = dataCache.Get<UserSalesModel>(DataCacheKey.Sales, dateTime);

            Assert.AreEqual(null,userSalesCache.User_Name);
        }
    }
}