using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Lab4.Task1.Service
{
    public class YearValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (!int.TryParse((string)value, out int year))
            {
                return new ValidationResult(false, "Рік повинен бути числом");
            }
            return ValidationResult.ValidResult;
        }
    }

}
