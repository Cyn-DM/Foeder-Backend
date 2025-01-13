namespace FoederDomain.CustomExceptions;

public class HouseholdNotFoundException : Exception
{
    public override string Message => "Household not found";
}