using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoederDomain.DomainModels;

public class Household
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Required]
    public Guid Id { get; set; }
    [Required, MinLength(1), MaxLength(50)]
    public string Name { get; set; } = string.Empty;
    [Required]
    public List<User> Users { get; set; } = new List<User>();

    public List<Recipe> Recipes { get; set; } = new List<Recipe>();
    
}