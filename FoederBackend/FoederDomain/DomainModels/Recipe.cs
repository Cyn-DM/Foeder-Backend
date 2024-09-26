namespace FoederDomain.DomainModels;

public class Recipe
{

    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<Ingredient> Ingredients { get; set; } = new();
    public List<string> Steps { get; set; } = new();

    public Recipe(){}
    
}

public class Ingredient
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Amount { get; set; } = string.Empty;
    
    public Ingredient(){}
}