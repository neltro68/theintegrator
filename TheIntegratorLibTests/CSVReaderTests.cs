using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TheIntegratorLib.Models;
using TheIntegratorLib.Utilities;

namespace TheIntegratorLibTests
{
    [TestClass]
    public class CSVReaderTests
    {       
        private enum _fields
        {
            user_name,
            age,
            height,
            gender,
            sale_amount,
            last_purchase_date
        }
        [DataTestMethod]
        [DataRow("user_name,age,height,gender,sale_amount,last_purchase_date")]
        public void CSVReader_GetHeader_Tests(string headers)
        {
            ICSVReader csvReader = new CSVReader();

            IEnumerable<string> header = csvReader.GetHeader(headers);

            Assert.AreEqual("user_name", header.ToArray()[0]);
            Assert.AreEqual("age", header.ToArray()[1]);
            Assert.AreEqual("height", header.ToArray()[2]);
            Assert.AreEqual("gender", header.ToArray()[3]);
            Assert.AreEqual("sale_amount", header.ToArray()[4]);
            Assert.AreEqual("last_purchase_date", header.ToArray()[5]);
        }

        [DataTestMethod]
        [DataRow("user_name,sale_amount,last_purchase_date,age,height,gender")]
        public void CSVReader_SetHeader_Tests(string headers)
        {
            ICSVReader csvReader = new CSVReader();

            IEnumerable<string> header = csvReader.GetHeader(headers);
            csvReader.SetHeader(header);
            Dictionary<int, string> headervalues = csvReader.GetHeader();

            Assert.AreEqual("user_name", headervalues[(int)_fields.user_name]);
            Assert.AreEqual("sale_amount", headervalues[(int)_fields.sale_amount]);
            Assert.AreEqual("last_purchase_date", headervalues[(int)_fields.last_purchase_date]);
            Assert.AreEqual("age", headervalues[(int)_fields.age]);
            Assert.AreEqual("height", headervalues[(int)_fields.height]);
            Assert.AreEqual("gender", headervalues[(int)_fields.gender]);
        }

        [DataTestMethod]
        [DataRow("John Doe, 29, 177, M, 21312, 2020-11-05T13:15:30Z")]
        public void CSVReader_ReadRow_Tests(string row)
        {
            ICSVReader csvReader = new CSVReader();

            IEnumerable<string> header = csvReader.GetHeader("user_name,sale_amount,last_purchase_date,age,height,gender");
            csvReader.SetHeader(header);
            UserSalesModel model = csvReader.ReadRow<UserSalesModel>(row);

            Assert.AreEqual(true, csvReader.HasHeader());
            Assert.AreEqual("John Doe", model.User_Name);
            Assert.AreEqual(29, model.Age);
            Assert.AreEqual(177, model.Height);
            Assert.AreEqual("M", model.Gender);
            Assert.AreEqual(21312, model.Sale_Amount);
            Assert.AreEqual(Convert.ToDateTime("2020-11-05T13:15:30Z"), model.Last_Purchase_Date);
        }
    }
}