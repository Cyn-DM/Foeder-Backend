using System.ComponentModel.DataAnnotations;
using FoederBusiness.Helpers;
using FoederDomain.CustomExceptions;

namespace FoederBusiness.Tools;

public static class ValidationUtils
{
    public static void ValidateObject<T>(T obj)
    where T : class
    {
        List<ValidationResult> validationResults = new List<ValidationResult>();
        Validator.TryValidateObject(obj, new ValidationContext(obj), validationResults, true);

        if (validationResults.Any())
        {
            throw new InvalidObjectException(validationResults);
        }
    }
}