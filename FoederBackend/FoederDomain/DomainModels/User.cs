namespace FoederDomain.DomainModels;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    [Required, MinLength(1), MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;
    [Required, MinLength(1), MaxLength(60)]
    public string LastName { get; set; } = string.Empty;
    [NotMapped]
    public string FullName => FirstName + " " + LastName;
    [Key, EmailAddress, MinLength(3), MaxLength(320)]
    public string Email { get; set; } = string.Empty;
    [Required]
    public Household Household { get; set; }

    public User(){}
}