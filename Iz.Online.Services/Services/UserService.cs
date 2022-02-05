using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iz.Online.ExternalServices.Rest.ExternalService;
using Iz.Online.Reopsitory.IRepository;
using Iz.Online.Services.IServices;
using Izi.Online.ViewModels.ShareModels;
using Izi.Online.ViewModels.Trades;

namespace Iz.Online.Services.Services
{
    public class UserService : IUserService
    {
        public IUserRepository _userRepository { get; set; }
        public IExternalUserService _externalUserService { get; set; }

        public UserService(IUserRepository userRepository, IExternalUserService externalUserService)
        {
            _userRepository = userRepository;
            _externalUserService = externalUserService;
        }

        public List<string> UserHubsList(string UserId)
        {

            return _userRepository.GetUserHubs(UserId);

        }
        public List<Trade> Trades(ViewBaseModel viewBaseMode)
        {
            var trades = _externalUserService.Trades(viewBaseMode);
            var allTrades = trades.Trades.Where(t => t.TradedAt == DateTime.Today).Select(x => new Trade()
            {
                Name = x.Order.instrument.name,
                Price = x.Price,
                State = x.Order.state,
                OrderSide = x.Order.orderSide,
                ExecutedQ = x.Order.executedQ,
                TradedAt = x.TradedAt
            }).ToList();
           
            return allTrades;
        }
    }
}
