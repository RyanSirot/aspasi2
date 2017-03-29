using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZenithSociety.Models.CustomValidation
{
    class DateRange : ValidationAttribute
    {
        private readonly DateTime _minDate;
        private readonly DateTime _maxDate;

        public DateRange() : base("{0} exceeds the value of valid date.")
        {
            _minDate = DateTime.Now.AddYears(-20);
            _maxDate = DateTime.Now.AddYears(20);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                DateTime date = (DateTime)value;

                if (date < _minDate || date > _maxDate)
                {
                    var errorMessage = FormatErrorMessage(validationContext.DisplayName);
                    return new ValidationResult(errorMessage);
                }
            }

            return ValidationResult.Success;
        }


    }
}
