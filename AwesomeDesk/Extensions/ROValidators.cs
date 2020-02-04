using AwesomeDesk.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AwesomeDesk.Extensions
{
    public class UniqueCompanyName : ValidationAttribute{
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var thiscmp = validationContext.ObjectInstance as Company;
            if (thiscmp == null) return new ValidationResult("Model is empty");
            ApplicationDbContext db = new ApplicationDbContext();
            var cmp = db.Companies.FirstOrDefault(u => u.CmP_Name == (string)value && u.CmP_ID !=thiscmp.CmP_ID);
            if (cmp == null)
                return ValidationResult.Success;
            else
                return new ValidationResult("Podana firma już istnieje!");
        }
    }
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DateTimeNotLessThan : ValidationAttribute, IClientValidatable
    {
        private const string DefaultErrorMessage = "{0} nie może być wcześniejsza niż {1}.";

        public string OtherProperty { get; private set; }
        private string OtherPropertyName { get; set; }

        public DateTimeNotLessThan(string otherProperty, string otherPropertyName)
            : base(DefaultErrorMessage)
        {
            if (string.IsNullOrEmpty(otherProperty))
            {
                throw new ArgumentNullException("otherProperty");
            }

            OtherProperty = otherProperty;
            OtherPropertyName = otherPropertyName;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, name, OtherPropertyName);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var otherProperty = validationContext.ObjectInstance.GetType().GetProperty(OtherProperty);
                var otherPropertyValue = otherProperty.GetValue(validationContext.ObjectInstance, null);

                DateTime dtThis = Convert.ToDateTime(value);
                DateTime dtOther = Convert.ToDateTime(otherPropertyValue);

                if (dtThis < dtOther)
                {
                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                }
            }
            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(
                                                      ModelMetadata metadata,
                                                      ControllerContext context)
        {
            var clientValidationRule = new ModelClientValidationRule()
            {
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = "notlessthan"//
            };

            clientValidationRule.ValidationParameters.Add("otherproperty", OtherProperty);

            return new[] { clientValidationRule };
        }
    }
}
