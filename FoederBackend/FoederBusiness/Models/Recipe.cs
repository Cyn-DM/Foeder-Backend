using FoederDomain.DomainModels;


public class Recipe
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<Ingredient> Ingredients { get; set; }
    public List<string> Steps { get; set; }
    public Household Household { get; set; }

    public Recipe(){}
    
}

public class Ingredient
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Amount { get; set; } = string.Empty;
    
    public Ingredient(){}
}