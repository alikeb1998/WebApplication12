using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iz.Online.HubConnectionHandler.IServices;

using Microsoft.AspNetCore.SignalR;
namespace Iz.Online.SignalR
{
    public class CustomersHub : Hub
    {
        private readonly IHubConnationService _hubConnationService;
        public CustomersHub(IHubConnationService userService)
        {
            _hubConnationService = userService;
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);

            //var hubs = _hubConnationService.UserHubsList("user1").Select(x=>x.HubId);

            //await Clients.Clients(hubs).SendAsync("ReceiveMessage", user + "hhhhh", message + "hhhhh");
        }
        public override async Task OnConnectedAsync()
        {
            //_hubConnationService.SetUserHub("user1", Context.ConnectionId);
            //var t = _hubConnationService.UserHubsList("user1");
            
            //await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");
            //await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _hubConnationService.DeleteConnectionId( Context.ConnectionId);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
