namespace FoederDomain.CustomExceptions;

public class RecipeNotFoundException : Exception
{
    public override string Message => "Recipe not found";
}