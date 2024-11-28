using System.ComponentModel.DataAnnotations;
using FoederBusiness.Helpers;

namespace FoederBusiness.Tools;

public static class ValidationUtils
{
    public static ValidationDTO ValidateObject<T>(T obj)
    where T : class
    {
        List<ValidationResult> validationResults = new List<ValidationResult>();
        Validator.TryValidateObject(obj, new ValidationContext(obj), validationResults, true);

        var dto = new ValidationDTO();
        
        if (validationResults.Any())
        {
            dto.hasOperationSucceeded = false;
            dto.ValidationResults = validationResults;
        }

        return dto;
    }
}