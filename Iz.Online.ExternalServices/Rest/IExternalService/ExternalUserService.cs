using Iz.Online.ExternalServices.Rest.ExternalService;
using Iz.Online.ExternalServices.Rest.Infrastructure;
using Iz.Online.OmsModels.InputModels;
using Iz.Online.OmsModels.InputModels.Users;
using Iz.Online.OmsModels.ResponsModels.Order;
using Iz.Online.OmsModels.ResponsModels.User;
using Iz.Online.OmsModels.Users.InputModels;
using Iz.Online.Reopsitory.IRepository;
using Izi.Online.ViewModels.ShareModels;
using Izi.Online.ViewModels.Users;
using Newtonsoft.Json;
using model = Iz.Online.OmsModels.ResponsModels.User;
using OtpResult = Iz.Online.OmsModels.ResponsModels.User.OtpResult;
using Wallet = Iz.Online.OmsModels.ResponsModels.User.Wallet;

namespace Iz.Online.ExternalServices.Rest.IExternalService
{
    public class ExternalUserService : BaseService, IExternalUserService
    {

        public ExternalUserService(IBaseRepository baseRepository) : base(baseRepository)
        {
        }

        public ResultModel<Wallet> Wallet()
        {
            var result = HttpGetRequest<Wallet>("user/wallet");

            return new ResultModel<Wallet>(result, result.statusCode == 200, result.clientMessage, result.statusCode);
        }

        public ResultModel<AssetsList> GetAllAssets()
        {
            var result = HttpGetRequest<AssetsList>("order/asset/all");

            return new ResultModel<AssetsList>(result, result.statusCode == 200, result.clientMessage, result.statusCode);

        }
        public Login Captcha()
        {
            var result = HttpGetRequest<Login>("user/captcha");
            return result;
        }

        public OtpResult SendOtp(Credentials credentials)
        {
            var result = HttpPostRequest<OtpResult>("user/login/send-otp", JsonConvert.SerializeObject(credentials));
            return result;
        }

        public CheckOtp CheckOtp(Otp otp)
        {
            var result = HttpPostRequest<CheckOtp>("user/login/check-otp", JsonConvert.SerializeObject(otp));
            return result;
        }
    }
}
