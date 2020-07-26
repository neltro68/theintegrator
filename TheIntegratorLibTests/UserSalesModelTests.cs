using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TheIntegratorLib.Models;
using TheIntegratorLib.Services;
using TheIntegratorLib.Utilities;

namespace TheIntegratorLibTests
{
    [TestClass]
    public class UserSalesModelTests
    {       
        [TestMethod]
        public void ValidateUserSalesModel_Tests()
        {
            var user = new UserSalesModel();

            user.User_Name = "123456789012345678901234567890123456789012345678901";
            user.Age = 91;
            user.Height = 501;
            user.Gender = "Z";

            IList<ValidationResult> validateResult = ValidateModel(user);

            Assert.AreEqual(4, validateResult.Count);
            Assert.IsTrue(ValidateModel(user).Any(
             v => v.MemberNames.Contains("Gender") &&
                  v.ErrorMessage.Contains("Valid values must be M or F")));
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