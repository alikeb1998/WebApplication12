using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iz.Online.HubConnectionHandler.IServices;

using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Iz.Online.SignalR
{
    public class CustomersHub : Hub
    {
        private readonly IHubConnationService _hubConnationService;
        private readonly ILogger<CustomersHub> _logger;
        public CustomersHub(IHubConnationService userService, ILogger<CustomersHub> logger)
        {
            _hubConnationService = userService;
            _logger=logger;
            _logger.LogError("CustomersHub init :" + DateTime.Now.ToString());

        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        public async Task SendMessageClients(string methodName, List<string> clients, Object[] o)
        {
            await Clients.Clients(clients).SendAsync(methodName, o);
        }
        public async Task ReceiveMessage(string user, string message)
        {
            //await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        public override async Task OnConnectedAsync()
        {
            _logger.LogError("OnConnectedAsync :" + DateTime.Now.ToString());
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            // _hubConnationService.DeleteConnectionId(Context.ConnectionId);

            _logger.LogError("OnDisconnectedAsync :"+DateTime.Now.ToString());
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
