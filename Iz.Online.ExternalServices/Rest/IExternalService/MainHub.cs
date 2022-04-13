using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iz.Online.HubConnectionHandler.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Iz.Online.ExternalServices.Rest.IExternalService
{
    public class MainHub : Hub
    {
        private readonly IHubConnationService _hubConnationService;
        private readonly ILogger<MainHub> _logger;
        public static string NatCode { get; set; }
        public MainHub(IHubConnationService userService, ILogger<MainHub> logger)
        {
            _hubConnationService = userService;
            _logger = logger;
            _logger.LogError("CustomersHub init :" + DateTime.Now.ToString());


        }

        public async Task AddToInstrumentsGroup(int instrumentId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"{instrumentId}/{NatCode}");
        }    
        public async Task AddToNewOrderGroup(string instrumentId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"NewOrder{NatCode}");
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Order/{instrumentId}/{NatCode}");
        }
        public async Task AddToOnlineUserGroup(string nationalCode)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"{nationalCode}");
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
             AddToOnlineUserGroup(NatCode);
            return base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
           
            //_hubConnationService.DeleteConnectionId(Context.ConnectionId);
            _logger.LogError("OnDisconnectedAsync :" + DateTime.Now.ToString());
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnDisconnectedAsync(exception);
        }
    }
}


