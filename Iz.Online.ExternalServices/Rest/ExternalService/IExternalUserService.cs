

using Iz.Online.OmsModels.InputModels;
using Iz.Online.OmsModels.ResponsModels.User;
using Izi.Online.ViewModels.ShareModels;
using Iz.Online.OmsModels.ResponsModels.User;
using Iz.Online.OmsModels.ResponsModels.Order;
using Iz.Online.OmsModels.Users.InputModels;
using Iz.Online.OmsModels.InputModels.Users;

namespace Iz.Online.ExternalServices.Rest.ExternalService
{
    public interface IExternalUserService
    {
        string Token { get; set; }
        ResultModel<Wallet> Wallet();
        ResultModel<AssetsList>  GetAllAssets();
        ResultModel<Login> Captcha();
        ResultModel<OtpResult> SendOtp(Credentials credentials);
        ResultModel<CheckOtp> CheckOtp(Otp otp);
        ResultModel<LogOut> LogOut();
    }
}
