namespace FoederBusiness.Services;

public class InvalidBearerTokenException : Exception
{
    public override string Message => "Invalid bearer token";
}