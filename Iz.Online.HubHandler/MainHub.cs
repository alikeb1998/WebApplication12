using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iz.Online.HubConnectionHandler.IServices;
using Iz.Online.Reopsitory.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Iz.Online.HubHandler
{
    public class MainHub : Hub
    {
        private readonly IHubConnationService _hubConnationService;
        private readonly ILogger<MainHub> _logger;
        public static string NatCode { get; set; }
        private readonly IInstrumentsRepository _instrumentsRepository;

        public MainHub(IHubConnationService userService, ILogger<MainHub> logger, IInstrumentsRepository instrumentsRepository)
        {
            _hubConnationService = userService;
            _logger = logger;
            _logger.LogError("CustomersHub init :" + DateTime.Now.ToString());
            _instrumentsRepository = instrumentsRepository;

        }

        public async Task AddToInstrumentsGroup(int instrumentId)
        {
            var nsc = _instrumentsRepository.InstrumentData(instrumentId);
         
            await Groups.AddToGroupAsync(Context.ConnectionId, $"instruments/{nsc}");
        }  
        public async Task RemoveFromInstrumentsGroup(int instrumentId)
        {
            var nsc = _instrumentsRepository.InstrumentData(instrumentId);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"instruments/{nsc}");
        }
        //public async Task AddToNewOrderGroup(int instrumentId)
        //{
            
        //    var nsc = _instrumentsRepository.InstrumentData(instrumentId);
        //    _logger.LogError("AddToNewOrderGroup" + instrumentId+nsc);
        //    await Groups.AddToGroupAsync(Context.ConnectionId, $"NewOrder/{NatCode}/{nsc}");
        //    await Groups.AddToGroupAsync(Context.ConnectionId, $"Order/{NatCode}/{nsc}");
        //} 
        //public async Task AddToPricesGroup(int instrumentId)
        //{
            
        //    var nsc = _instrumentsRepository.InstrumentData(instrumentId);
        //    _logger.LogError("AddToPricesGroup" + instrumentId + nsc);
        //    await Groups.AddToGroupAsync(Context.ConnectionId, $"price/{NatCode}/{nsc}");
        //}
        public async Task AddToOnlineUserGroup(string nationalCode)
        {
            
            await Groups.AddToGroupAsync(Context.ConnectionId, $"{nationalCode}");
        }
        public async Task RemoveFromOnlineUserGroup(string nationalCode)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"{nationalCode}");
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
            await AddToOnlineUserGroup(NatCode);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {

            //_hubConnationService.DeleteConnectionId(Context.ConnectionId);
            await RemoveFromOnlineUserGroup(NatCode);
            _logger.LogError("OnDisconnectedAsync :" + DateTime.Now.ToString());
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnDisconnectedAsync(exception);
        }
    }
}


