using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ContosoUniversityWebApplication.Attribute
{

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    sealed public class CustomAttribute : ValidationAttribute
    {

        public override bool IsValid(object value)
        {
            bool result = true;
            // Add validation logic here.
            return result;
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentCulture,
              ErrorMessageString, name);
        }

    }

}