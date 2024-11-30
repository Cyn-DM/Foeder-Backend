namespace FoederDomain.CustomExceptions;

public class UserNotFoundException : Exception
{
    public override string Message => "User Not Found";
}