using System.ComponentModel.DataAnnotations;

namespace FoederDomain.DomainModels;

public class Recipe
{
    [Key]
    public Guid Id { get; set; }
    [MaxLength(100), MinLength(1), Required]
    public string Title { get; set; } = string.Empty;
    [MaxLength(250)]
    public string Description { get; set; } = string.Empty;
    [Required]
    public List<Ingredient> Ingredients { get; set; } = new();
    public List<string> Steps { get; set; } = new();
    [Required]
    public Household Household { get; set; }

    public Recipe(){}
    
}

public class Ingredient
{
    [Key]
    public Guid Id { get; set; }
    [Required, MinLength(1), MaxLength(50)]
    public string Name { get; set; } = string.Empty;
    [Required, MinLength(1), MaxLength(50)]
    public string Amount { get; set; } = string.Empty;
    
    public Ingredient(){}
}