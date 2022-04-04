

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
        Task<ResultModel<Wallet>> Wallet();
        Task<ResultModel<AssetsList>> GetAllAssets();
        Task<ResultModel<Login>> Captcha();
        Task<ResultModel<OtpResult>> SendOtp(Credentials credentials);
        Task<ResultModel<CheckOtp>> CheckOtp(Otp otp);
        Task<ResultModel<LogOut>> LogOut();
        Task<ResultModel<CustomerInfo>> CustomerInfo();
    }
}
