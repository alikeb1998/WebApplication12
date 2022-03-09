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
        }

        public override async Task OnConnectedAsync()
        {
          
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _hubConnationService.DeleteConnectionId(Context.ConnectionId);
           
            
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
