using Izi.Online.ViewModels.ShareModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using  Izi.Online.ViewModels.Trades;
using Asset = Izi.Online.ViewModels.Orders.Asset;
using Izi.Online.ViewModels.Users;
using Iz.Online.OmsModels.InputModels;
using Iz.Online.OmsModels.Users.InputModels;
using Iz.Online.OmsModels.InputModels.Users;
using Izi.Online.ViewModels.Reports;
using Iz.Online.ExternalServices.Rest.ExternalService;

namespace Iz.Online.Services.IServices
{
    public interface IUserService
    {
        IExternalUserService _externalUserService { get; }
        void SetUserHub(string token, string hubId);

        List<UsersHubIds> UserHubsList(string UserId);
        ResultModel<List<Asset>> AllAssets();
        ResultModel<List<Asset>> AllAssetsPaged(PortfoFilter filter);
        ResultModel<Wallet>  Wallet();
        List<Izi.Online.ViewModels.AppConfigs> AppConfigs();

        ResultModel<string> GetUserLocalToken(string  omsId);

        ResultModel<Captcha> Captcha();
        ResultModel<OtpResult> SendOtp(Credentials credentials);
        ResultModel<CheckedOtp> CheckOtp(Otp otp);
        ResultModel<bool> LogOut();

    }
}
