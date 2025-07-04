﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FoederDomain.CustomValidationAttributes;

namespace FoederDomain.DomainModels;

public class Recipe
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Required]
    public Guid Id { get; set; }
    [MaxLength(100), MinLength(1), Required]
    public string Title { get; set; } = string.Empty;
    [MaxLength(250)]
    public string Description { get; set; } = string.Empty;
    [Required, NotEmpty]
    public List<Ingredient> Ingredients { get; set; } = new();
    [MaxLength(250)]
    public List<string> Steps { get; set; } = new();
    [Required]
    public Guid HouseholdId { get; set; }
    public Household? Household { get; set; }

    public Recipe(){}
    
}

public class Ingredient
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Required]
    public Guid Id { get; set; }
    [Required, MinLength(1), MaxLength(50)]
    public string Name { get; set; } = string.Empty;
    [Required, MinLength(1), MaxLength(50)]
    public string Amount { get; set; } = string.Empty;

    public Guid RecipeId { get; set; }

    public Ingredient(){}
}