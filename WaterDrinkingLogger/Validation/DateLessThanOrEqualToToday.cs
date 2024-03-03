using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace WaterDrinkingLogger.Validation;

public class DateLessThanOrEqualToToday : ValidationAttribute
{
    public override string FormatErrorMessage(string name)
    {
        return "Date value should not be a future date";
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var dateValue = value as DateOnly? ?? new DateOnly();

        if (dateValue.CompareTo(DateOnly.FromDateTime(DateTime.Now.Date)) > 0)
            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));

        return ValidationResult.Success;
    }
}
