using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TheIntegratorLib.Models;

namespace TheIntegratorLibTests
{
    [TestClass]
    public class UserSalesModelTests
    {       
        [TestMethod]
        public void ValidateUserSalesModel_Tests()
        {
            var user = new UserSalesModel
            {
                User_Name = "123456789012345678901234567890123456789012345678901",
                Age = 91,
                Height = 501,
                Gender = "Z"
            };

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
            _ = Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }
    }
}