using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iz.Online.HubHandler.IServices;
using Microsoft.AspNetCore.SignalR;
namespace Iz.Online.SignalR
{
    public class CustomersHub : Hub
    {
        private readonly IHubUserService _userService;
        public CustomersHub(IHubUserService userService)
        {
            _userService = userService;
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);

            var hubs = _userService.UserHubsList("user1").HubId;

            await Clients.Clients(hubs).SendAsync("ReceiveMessage", user + "hhhhh", message + "hhhhh");
        }
        public override async Task OnConnectedAsync()
        {
            _userService.SetUserHub("user1", Context.ConnectionId);
            var t = _userService.UserHubsList("user1");

            await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _userService.DeleteConnectionId("user1", Context.ConnectionId);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
