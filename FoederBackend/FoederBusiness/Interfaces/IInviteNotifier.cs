namespace FoederBusiness.Interfaces;

public interface IInviteNotifier
{
    Task NotifyInvite(Guid invitedUser, string message);
}