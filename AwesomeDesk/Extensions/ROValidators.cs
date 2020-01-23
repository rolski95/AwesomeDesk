using AwesomeDesk.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AwesomeDesk.Extensions
{
    public class UniqueCompanyName : ValidationAttribute
    {
   


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
}