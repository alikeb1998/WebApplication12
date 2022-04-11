using Amazon.Auth.AccessControlPolicy;
using Iz.Online.HubConnectionHandler.IServices;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.ExternalServices.Rest.IExternalService
{
    public class MainHub : Hub
    {
        private readonly IHubConnationService _hubConnationService;
        public MainHub(IHubConnationService userService)
        {
            _hubConnationService = userService;
        }
        //[Resource("AddToUserGroup")]
        public async Task AddToUserGroup(string nationalCode)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"instruments{nationalCode}");
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
        public override Task OnConnectedAsync()
        {
            var a = Context;
            //await Groups.AddToGroupAsync(Context.ConnectionId, $"instruments{userInfo.nationalCode}");
           
            return base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            //_hubConnationService.DeleteConnectionId(Context.ConnectionId);
          
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
