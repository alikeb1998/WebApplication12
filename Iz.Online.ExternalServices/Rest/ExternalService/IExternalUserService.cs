

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
        ResultModel<Wallet> Wallet();

        ResultModel<AssetsList>  GetAllAssets();
        Login Captcha();
        OtpResult SendOtp(Credentials credentials);
        CheckOtp CheckOtp(Otp otp);
        ResultModel<LogOut> LogOut();
    }
}
