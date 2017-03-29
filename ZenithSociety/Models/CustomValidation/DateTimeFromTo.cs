using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ZenithSociety.Models.Business;

namespace ZenithSociety.Models.CustomValidation
{
    class DateTimeFromTo : ValidationAttribute
    {

        public DateTimeFromTo() : base("{0} is greater than the Date of Event From")
        {

        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Event eventModel = (Event)validationContext.ObjectInstance;

            if (value != null)
            {

                var dateTo = (DateTime)value;
                var dateFrom = eventModel.EventFrom;
                if (dateTo <= dateFrom)
                {
                    var errorMessage = FormatErrorMessage(validationContext.DisplayName);
                    return new ValidationResult(errorMessage);
                }

            }
            return ValidationResult.Success;
        }
    }
}
