using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace FoederDomain.CustomValidationAttributes;

public class NotEmptyAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is IEnumerable enumerable && !HasElements(enumerable))
        {
            return new ValidationResult($"{validationContext.DisplayName} cannot be empty.");
        }
        
        return ValidationResult.Success;
    }

    private bool HasElements(IEnumerable enumerable)
    {
        foreach (var element in enumerable)
        {
            return true;
        }

        return false;
    }
}