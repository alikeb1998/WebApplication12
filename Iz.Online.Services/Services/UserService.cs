using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iz.Online.ExternalServices.Rest.ExternalService;
using Iz.Online.OmsModels.InputModels;
using Iz.Online.Reopsitory.IRepository;
using Iz.Online.Services.IServices;
using Izi.Online.ViewModels;
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
        public ResultModel<List<Asset>> AllAssets(ViewBaseModel viewBaseModel)
        {
            var assets = _externalUserService.GetAllAssets(viewBaseModel);

            if (!assets.IsSuccess)
                return new ResultModel<List<Asset>>(null, false, assets.Message, assets.StatusCode);

            var result = assets.Model.Assets.Select(x => new Asset()
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
            return new ResultModel<List<Asset>>(result);

        }

        public ResultModel<Wallet> Wallet(ViewBaseModel model)
        {
            var respond = _externalUserService.Wallet(new OmsBaseModel()
            {
                Authorization = model.Token
            });

            if (!respond.IsSuccess)
                return new ResultModel<Wallet>(null, false, respond.Message, respond.StatusCode);

            var result = new Wallet()
            {
                Withdrawable = respond.Model.wallet.withdrawable,
                BlockedValue = respond.Model.wallet.blockedValue,
                BuyingPower = respond.Model.wallet.buyingPower,
                LendedCredit = respond.Model.wallet.lendedCredit,
                NonWithdrawable = respond.Model.wallet.nonWithdrawable,
            };
           
            return new ResultModel<Wallet>(result);
        }

        public List<AppConfigs> AppConfigs()
        {
            return _userRepository.GetAppConfigs().Select(x => new AppConfigs()
            {
                Key = x.Key,
                Description = x.Description,
                Value = x.Value
            }).ToList();
        }

        public AppConfigs AppConfigs(string key)
        {
            throw new NotImplementedException();
        }
    }
}
