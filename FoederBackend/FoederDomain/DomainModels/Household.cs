namespace FoederDomain.DomainModels;

public class Household
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    [Key]
    public Guid Id { get; set; }
    [Required, MinLength(1), MaxLength(50)]
    public string Name { get; set; } = string.Empty;
    [Required]
    public List<User> Users { get; set; }

    public Household(){}
}