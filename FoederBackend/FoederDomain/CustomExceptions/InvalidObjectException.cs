using System.ComponentModel.DataAnnotations;

namespace FoederDomain.CustomExceptions;

public class InvalidObjectException(List<ValidationResult> validationResults) : Exception
{
    public override string Message => "Object did not pass validation";
    public List<ValidationResult> ValidationResults { get; set; } = validationResults; 
}