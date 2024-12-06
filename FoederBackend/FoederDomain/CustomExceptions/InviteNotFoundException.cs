namespace FoederDAL.Repository;

public class InviteNotFoundException : Exception
{
    public override string Message => "Invite not found";
}