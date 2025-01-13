using FoederBusiness.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace FoederAPI.Hubs;

public class InviteHub : Hub
{
    public override Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        Console.WriteLine($"User connected: {userId}");
        
        if (string.IsNullOrEmpty(userId))
        {
            Console.WriteLine("No user identifier found. Check claims:");
            foreach (var claim in Context.User.Claims)
            {
                Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
            }
        }

        return base.OnConnectedAsync();
    }
}

public class SignalRInviteNotifier : IInviteNotifier
{
    private readonly IHubContext<InviteHub> _hub;

    public SignalRInviteNotifier(IHubContext<InviteHub> hub)
    {
        _hub = hub;
    }

    public async Task NotifyInvite(Guid invitedUser, string message)
    {
        Console.WriteLine($"Sending invite notification to user: {invitedUser}, message: {message}");
        await _hub.Clients.User(invitedUser.ToString()).SendAsync("ReceiveInvite", message);
        Console.WriteLine($"Notification sent to user: {invitedUser}");
    }
}

public class CustomUserIdProvider : IUserIdProvider
{
    public string GetUserId(HubConnectionContext connection)
    {
        return connection.User?.FindFirst("Id")?.Value;
    }
}