

using Iz.Online.OmsModels.InputModels;
using Iz.Online.OmsModels.ResponsModels.User;
using Izi.Online.ViewModels.ShareModels;
using Iz.Online.OmsModels.ResponsModels.User;
using Iz.Online.OmsModels.ResponsModels.Order;

namespace Iz.Online.ExternalServices.Rest.ExternalService
{
    public interface IExternalUserService
    {
        ResultModel<Wallet> Wallet();

        ResultModel<AssetsList>  GetAllAssets();
        Login Captcha();
        OtpResult SendOtp(Credentials credentials);
    }
}
