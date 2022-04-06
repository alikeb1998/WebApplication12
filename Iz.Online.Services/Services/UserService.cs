using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iz.Online.ExternalServices.Rest.ExternalService;
using Iz.Online.HubHandler.IServices;
using Iz.Online.OmsModels.InputModels;
using Iz.Online.OmsModels.InputModels.Users;
using Iz.Online.OmsModels.Users.InputModels;
using Iz.Online.Reopsitory.IRepository;
using Iz.Online.Services.IServices;
using Izi.Online.ViewModels;
using Izi.Online.ViewModels.Orders;
using Izi.Online.ViewModels.Reports;
using Izi.Online.ViewModels.ShareModels;
using Izi.Online.ViewModels.Users;
using Microsoft.IdentityModel.Tokens;

namespace Iz.Online.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHubUserService _hubUserService;
        private readonly ICacheService _cacheService;
        public IExternalUserService _externalUserService { get; }


        public UserService(IUserRepository userRepository, IExternalUserService externalUserService, IHubUserService hubUserService, ICacheService cacheService)
        {
            _userRepository = userRepository;
            _externalUserService = externalUserService;
            _cacheService = cacheService;
        }

        public async Task<ResultModel<List<Asset>>> AllAssets()
        {
            var assets = await _externalUserService.GetAllAssets();
            var instruments =  _cacheService.InstrumentData();

            if (!assets.IsSuccess || assets.Model.Assets == null)
                return new ResultModel<List<Asset>>(null, assets.StatusCode == 200, assets.Message, assets.StatusCode);

            var allAssets = assets.Model.Assets.Select(x => new Asset()
            {
                Name = x.Instrument.name,
                LastPrice = x.LastPrice,
                TradeableQuantity = x.TradeableQuantity,
                Gav = x.Gav,
                AvgPrice = x.AvgPrice,
                FianlAmount = x.FinalAmount,
                ProfitAmount = x.ProfitAmount,
                ProfitPercent = x.ProfitPercent,
                SellProfit = x.SellProfit,
                InstrumentId = _cacheService.GetLocalInstrumentIdFromOmsId(x.Instrument.id),
            }).ToList();

            return new ResultModel<List<Asset>>(allAssets);

        }

        public async Task<ResultModel<List<Asset>>> AllAssetsPaged(PortfoFilter filter)
        {
            var assets = await _externalUserService.GetAllAssets();

            if (!assets.IsSuccess || assets.Model.Assets == null)
                return new ResultModel<List<Asset>>(null, assets.StatusCode == 200, assets.Message, assets.StatusCode);

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
                SellProfit = x.SellProfit,
                InstrumentId = x.Instrument.id,
                NscCode = x.Instrument.code
            }).ToList();

            var res = Filter(result, filter);

            return new ResultModel<List<Asset>>(res);

        }
        public async Task<ResultModel<Wallet>> Wallet()
        {

            var respond = await _externalUserService.Wallet();

            if (!respond.IsSuccess || respond.Model.wallet == null)
                return new ResultModel<Wallet>(null, respond.StatusCode == 200, respond.Message, respond.StatusCode);

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
            return _userRepository.ConfigData();
        }

        public ResultModel<string> GetUserLocalToken(string token)
        {
            var deserializedToken = CastJwtSecurityTokenHandler(token);
            var omsId = ((JwtSecurityToken)deserializedToken).Claims.FirstOrDefault(x => x.Type == "Id").Value;

            string localToken =  _userRepository.GetUserLocalToken(omsId, token);

            return new ResultModel<string>(localToken);
        }

        private SecurityToken CastJwtSecurityTokenHandler(string stream)
        {
            var handler = new JwtSecurityTokenHandler();

            var jsonToken = handler.ReadToken(stream);

            return jsonToken;
        }

        public  async Task<ResultModel<bool>> SetUserHub(string token, string hubId)
        {
            var CustomerInfo = await _externalUserService.CustomerInfo();

            if (CustomerInfo.StatusCode == 200 || CustomerInfo.Model.tradingID != null)
            {
                bool setResult = _userRepository.SetUserInfo(new CustomerInfo()
                {
                    Token = token,
                    KafkaId = CustomerInfo.Model.tradingID,
                    Hubs = new List<string>() { hubId }
                });
                if (!setResult)
                    return new ResultModel<bool>(false, false, "خطا در دریافت اطلاعات کاربری", 404);

                return new ResultModel<bool>(true);
            }

            return new ResultModel<bool>(false, false, "خطا در دریافت اطلاعات کاربری", 404);

        }

        public async Task<ResultModel<Captcha>> Captcha()
        {
            var res = await _externalUserService.Captcha();
            if (res.Model != null && res.IsSuccess)
            {
                var captcha = new Captcha()
                {

                    CaptchaImage = res.Model.Captcha.Base64,
                    Id = res.Model.Captcha.Id

                };
                return new ResultModel<Captcha>(captcha);
            }
            return new ResultModel<Captcha>(null, false, res.Message, res.StatusCode);
        }

        public async Task<ResultModel<OtpResult>> SendOtp(Credentials credentials)
        {
            var result = await _externalUserService.SendOtp(credentials);
            if (result.Model != null && result.IsSuccess)
            {
                
                var OtpResult = new OtpResult()
                {
                    OtpId = result.Model.OtpId,
                    ExpireAt = result.Model.ExpireAt
                };
                return new ResultModel<OtpResult>(OtpResult);
            }
            return new ResultModel<OtpResult>(null, false, result.Message, result.StatusCode);
        }

        public async Task<ResultModel<CheckedOtp>> CheckOtp(Otp otp)
        {
            var result = await _externalUserService.CheckOtp(otp);
            if (result.Model != null && result.IsSuccess)
            {
                var checkOtp = new CheckedOtp()
                {
                    Token = result.Model.Token,
                    Sockettoken = result.Model.SocketToken
                };

                return new ResultModel<CheckedOtp>(checkOtp);
            }
            return new ResultModel<CheckedOtp>(null, false, result.Message, result.StatusCode);
        }
        public async Task<ResultModel<bool>> LogOut()
        {
            var res = await _externalUserService.LogOut();
            if (res.StatusCode == 200)
            {
                return new ResultModel<bool>(true);
            }
            return new ResultModel<bool>(false,false, res.Message, res.StatusCode);
          
        }

        public async Task<ResultModel<CustomerData>> GetCustomerInfo()
        {
            var result = await _externalUserService.CustomerInfo();

            if (!result.IsSuccess || result.Model == null)
                return new ResultModel<CustomerData>(null, result.IsSuccess, result.Message, result.StatusCode);
            return new ResultModel<CustomerData>(new CustomerData()
            {
                FullName = result.Model.nameFirst + " " + result.Model.nameLast,
                BourseCode = result.Model.borseCode,
                TradingId = result.Model.tradingID,
            }, result.IsSuccess, result.Message, result.StatusCode);


        }


        private List<Asset> Filter(List<Asset> list, PortfoFilter filter)
        {
            var report = new PortfolioReport()
            {
                Model = list.Skip(filter.PageSize * (filter.PageNumber - 1)).Take(filter.PageSize).ToList(),
                PortfolioSortColumn = filter.PortfolioSortColumn,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalCount = list.Count,
                OrderType = filter.OrderType,
            };

            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.PortfolioSortColumn)
                    {
                        case PortfolioSortColumn.Symbol:
                            return report.Model.OrderBy(x => x.Name).ToList();

                        case PortfolioSortColumn.Count:
                            return report.Model.OrderBy(x => x.TradeableQuantity).ToList();

                        case PortfolioSortColumn.ProfitOrLoss:
                            return report.Model.OrderBy(x => x.ProfitPercent).ToList();

                        case PortfolioSortColumn.Value:
                            return report.Model.OrderBy(x => x.Gav).ToList();

                    }
                    break;

                case OrderType.DESC:
                    switch (filter.PortfolioSortColumn)
                    {
                        case PortfolioSortColumn.Symbol:
                            return report.Model.OrderByDescending(x => x.Name).ToList();

                        case PortfolioSortColumn.Count:
                            return report.Model.OrderByDescending(x => x.TradeableQuantity).ToList();

                        case PortfolioSortColumn.ProfitOrLoss:
                            return report.Model.OrderByDescending(x => x.ProfitPercent).ToList();

                        case PortfolioSortColumn.Value:
                            return report.Model.OrderByDescending(x => x.Gav).ToList();

                    }
                    break;

                default:
                    return report.Model.OrderBy(x => x.TradeableQuantity).ToList();
            }
            return report.Model.OrderBy(x => x.TradeableQuantity).ToList();
        }
    }
}
