using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iz.Online.ExternalServices.Rest.ExternalService;
using Iz.Online.OmsModels.InputModels;
using Iz.Online.Reopsitory.IRepository;
using Iz.Online.Services.IServices;
using Izi.Online.ViewModels.Orders;
using Izi.Online.ViewModels.ShareModels;
using Izi.Online.ViewModels.Trades;
using Izi.Online.ViewModels.Users;

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
        public List<Asset> AllAssets(ViewBaseModel viewBaseModel)
        {
            var assets = _externalUserService.GetAllAssets(viewBaseModel);
            var res = assets.Assets.Select(x => new Asset()
            {
                Name = x.Instrument.name,
                LastPrice = x.LastPrice,
                TradeableQuantity = x.TradeableQuantity,
                Gav = x.Gav,
                AvgPrice = x.AvgPrice,
                FianlAmount = x.FinalAmount,
                ProfitAmount = x.ProfitAmount,
                ProfitPercent = x.ProfitPercent,
                SellProfit = x.SellProfit
            }).ToList();

            return res;
        }

        public Wallet Wallet(ViewBaseModel model)
        {
            var respond = _externalUserService.Wallet(new OmsBaseModel()
            {
                Authorization = model.Token
            }); ;
            
            var result = new Wallet()
            {
                Withdrawable = respond.wallet.withdrawable,
                BlockedValue = respond.wallet.blockedValue,
                BuyingPower = respond.wallet.buyingPower,
                LendedCredit = respond.wallet.lendedCredit,
                NonWithdrawable = respond.wallet.nonWithdrawable,
            };
            return result;
        }
    }
}
