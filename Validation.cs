﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Employee_Schedule
{
    class Validation
    {
        public class StringNotEmpty: ValidationRule
        {
            public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
            {
                string aString = value.ToString();
                if (aString == "") return new ValidationResult(false, "String cannot be empty!");
                return new ValidationResult(true, null);
            }
        }
        public class StringMinLengthValidator: ValidationRule
        {
            public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
            {
                string aString = value.ToString();
                if (aString.Length < 3) return new ValidationResult(false, "String must have at least 3 characters!");
                return new ValidationResult(true, null);
            }
        }
        public class StringMaxLengthValidator: ValidationRule
        {
            public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
            {
                string aString = value.ToString();
                if (aString.Length > 1) return new ValidationResult(false, "String must have only one character!");
                return new ValidationResult(true, null);
            }
        }
    }
}
