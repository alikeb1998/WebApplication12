﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iz.Online.ExternalServices.Rest.ExternalService;
using Iz.Online.OmsModels.InputModels;
using Iz.Online.OmsModels.InputModels.Users;
using Iz.Online.OmsModels.Users.InputModels;
using Iz.Online.Reopsitory.IRepository;
using Iz.Online.Services.IServices;
using Izi.Online.ViewModels;
using Izi.Online.ViewModels.Orders;
using Izi.Online.ViewModels.ShareModels;
using Izi.Online.ViewModels.Trades;
using Izi.Online.ViewModels.Users;
using Microsoft.IdentityModel.Tokens;
using db = Iz.Online.Entities;

namespace Iz.Online.Services.Services
{
    public class UserService : IUserService
    {
        public IUserRepository _userRepository { get; set; }
        public IExternalUserService _externalUserService { get; set; }
        public string _token { get; set; }

        public UserService(IUserRepository userRepository, IExternalUserService externalUserService)
        {
            _userRepository = userRepository;
            _externalUserService = externalUserService;
        }

        public UsersHubIds UserHubsList(string userId)
        {

            return  _userRepository.GetUserHubs(userId);

        }
       
        public ResultModel<List<Asset>> AllAssets()
        {
            var assets = _externalUserService.GetAllAssets();

            if (!assets.IsSuccess)
                return new ResultModel<List<Asset>>(null, false, assets.Message, assets.StatusCode);
            
            if (assets.Model.Assets==null)
                return new ResultModel<List<Asset>>(null, false,"مدل خالی برگشته است", assets.StatusCode);

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
                InstrumentId =x.Instrument.id,
                NscCode = x.Instrument.code
            }).ToList();
            return new ResultModel<List<Asset>>(result);

        }

        public ResultModel<Wallet> Wallet()
        {
            
            var respond = _externalUserService.Wallet();

            if (!respond.IsSuccess )
                return new ResultModel<Wallet>(null, false, respond.Message, respond.StatusCode);

            if (respond.Model.wallet == null)
                return new ResultModel<Wallet>(null, false, "مدل خالی برگشته است", respond.StatusCode);

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
            return _userRepository.GetAppConfigs().Select(x => new Izi.Online.ViewModels.AppConfigs()
            {
                Description = x.Description,
                Key = x.Key,
                Value = x.Value
            }).ToList();
        }
                     
        public ResultModel<string> GetUserLocalToken(string token)
        {
            var deserializedToken = CastJwtSecurityTokenHandler(token);
            var omsId = ((JwtSecurityToken)deserializedToken).Claims.FirstOrDefault(x => x.Type == "Id").Value;

            string localToken = _userRepository.GetUserLocalToken(omsId,token);


            return new ResultModel<string>(localToken);
        }
      
        private SecurityToken CastJwtSecurityTokenHandler(string stream)
        {
            var handler = new JwtSecurityTokenHandler();

            var jsonToken = handler.ReadToken(stream);

            return jsonToken;
        }

        public void SetUserHub(string UserId, string hubId)
        {
           _userRepository.SetUserHub(UserId, hubId);
        }

        public Captcha Captcha()
        {
            var res = _externalUserService.Captcha();
            var captcha = new Captcha()
            {

                CaptchaImage = res.Captcha.Base64,
                Id = res.Captcha.Id
        
            };
            return captcha;
        }

        public OtpResult SendOtp(Credentials credentials)
        {
            var result = _externalUserService.SendOtp(credentials);
            var OtpResult = new OtpResult()
            {
                OtpId = result.OtpId,
                ExpireAt = result.ExpireAt
            };
            return OtpResult;
        }
        public CheckedOtp CheckOtp(Otp otp)
        {
            var result = _externalUserService.CheckOtp(otp);
            var checkOtp= new CheckedOtp()
            {
              Token = result.Token,
              Sockettoken = result.SocketToken
            };
            return checkOtp;
        }
    }
}
