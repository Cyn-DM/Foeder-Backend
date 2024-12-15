using System.ComponentModel.DataAnnotations;

namespace FoederBusiness.Helpers;

public class ValidationDTO
{
    public List<ValidationResult> ValidationResults { get; set; } = new List<ValidationResult>();
}