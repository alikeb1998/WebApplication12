﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iz.Online.HubConnectionHandler.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Iz.Online.SignalR
{
    public class UserInfo
    {
        public string NatCode { get; set; }
    }
    //[Authorize]
    public class CustomersHub : Hub//Hub<IClientCustomerHub>
    {
        
        private readonly IHubConnationService _hubConnationService;
        private readonly UserInfo _userInfo;
        private readonly ILogger<CustomersHub> _logger;
        //public IExternalUserService _externalUserService { get; }
        public CustomersHub(IHubConnationService userService, ILogger<CustomersHub> logger,UserInfo userInfo)
        {
            _hubConnationService = userService;
            _logger=logger;
            _logger.LogError("CustomersHub init :" + DateTime.Now.ToString());
            _userInfo=userInfo;

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
        public override  Task OnConnectedAsync()
        {
            var a = Context;
            //await Groups.AddToGroupAsync(Context.ConnectionId, $"instruments{userInfo.nationalCode}");
            _logger.LogError("OnConnectedAsync :" + DateTime.Now.ToString());
            return base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            //_hubConnationService.DeleteConnectionId(Context.ConnectionId);
            _logger.LogError("OnDisconnectedAsync :"+DateTime.Now.ToString());
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
