using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace FoederDomain.DomainModels;

public class User
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    [Required, MinLength(1), MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;
    [MinLength(1), MaxLength(60)]
    public string? LastName { get; set; } = string.Empty;
    [NotMapped]
    public string FullName => FirstName + " " + LastName;
    [EmailAddress, MinLength(3), MaxLength(320)]
    public string Email { get; set; } = string.Empty; 
    public Guid? HouseholdId { get; set; }
    public Household? Household { get; set; }

    public User(){}
}