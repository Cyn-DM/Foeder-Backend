using System.ComponentModel.DataAnnotations;

namespace FoederBusiness.Helpers;

public class ValidationDTO
{
    public bool hasOperationSucceeded { get; set; }
    public List<ValidationResult> ValidationResults { get; set; } = new List<ValidationResult>();
}