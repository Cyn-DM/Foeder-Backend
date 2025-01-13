namespace FoederBusiness.Services;

public class UserAlreadyHasHouseholdException : Exception
{
    public override string Message => "User has already has a household";
}