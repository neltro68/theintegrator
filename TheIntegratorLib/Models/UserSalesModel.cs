using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace TheIntegratorLib.Models
{
    public interface IUserSales
    {
        public string User_Name { get; }
        public int Age { get; }
        public int Height { get; }
        public string Gender { get; }
        public double Sale_Amount { get; }
        public DateTime Last_Purchase_Date { get; }
    }

    public class UserSalesModel : IUserSales
    {
        [MaxLength(50)]
        public string User_Name { get; set; }
        [Range(18, 70)]
        public int Age { get; set; }
        [Range(1, 500)]
        public int Height { get; set; }
        [ValidString(ValidValues = new string[] { "M", "F" }, ErrorMessage = "Valid values must be M or F.")]
        public string Gender { get; set; }
        public double Sale_Amount { get; set; }
        public DateTime Last_Purchase_Date { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class ValidStringAttribute : ValidationAttribute
    {
        public string[] ValidValues { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (ValidValues?.Contains(value?.ToString()) == true)
            {
                return ValidationResult.Success;
            }

            var msg = $"Valid values must be {string.Join(" or ", (ValidValues ?? new string[] { "No allowable values found" }))}.";
            IEnumerable<string> memberNames = new List<string> { validationContext.MemberName };
            return new ValidationResult(msg, memberNames);
        }
    }
}
