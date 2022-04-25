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

        public ExternalUserService(IBaseRepository baseRepository) : base(baseRepository, ServiceProvider.Oms)
        {
        }

        public async Task<ResultModel<Wallet>> Wallet()
        {
            var result = await HttpGetRequest<Wallet>("user/wallet");

            return new ResultModel<Wallet>(result, result.statusCode == 200, result.clientMessage, result.statusCode);
        }

        public async Task<ResultModel<AssetsList>> GetAllAssets()
        {
            var result = await HttpGetRequest<AssetsList>("order/asset/all");

            return new ResultModel<AssetsList>(result, result.statusCode == 200, result.clientMessage, result.statusCode);

        } 
        public async Task<ResultModel<List<Portfo>>> Portfolio()
        {
            var result = await HttpGetRequest<List<Portfo>>("order/portfolio/assets");
            if (result == null)
            {
                return new ResultModel<List<Portfo>>(null, 400);
            }

            return new ResultModel<List<Portfo>>(result);

        }
        public async Task<ResultModel<Login>> Captcha()
        {
            var result = await HttpGetRequest<Login>("user/captcha");
            return new ResultModel<Login>(result, result.statusCode == 200, result.clientMessage, result.statusCode);
        }

        public async Task<ResultModel<OtpResult>> SendOtp(Credentials credentials)
        {
            var result = await HttpPostRequest<OtpResult>("user/login/send-otp", JsonConvert.SerializeObject(credentials));
            return new ResultModel<OtpResult>(result, result.statusCode == 200, result.clientMessage, result.statusCode);
        }

        public async Task<ResultModel<CheckOtp>> CheckOtp(Otp otp)
        {
            var result = await HttpPostRequest<CheckOtp>("user/login/check-otp", JsonConvert.SerializeObject(otp));
            return new ResultModel<CheckOtp>(result, result.statusCode == 200, result.clientMessage, result.statusCode);
        }
        public async Task<ResultModel<LogOut>> LogOut()
        {
            var result = await HttpDeleteRequest<LogOut>("user/logout",null);

            return new ResultModel<LogOut>(result, result.statusCode == 200, result.clientMessage, result.statusCode);
        }

        public async Task<ResultModel<model.CustomerInfo>> CustomerInfo()
        {
          
            var result = await  HttpGetRequest<model.CustomerInfo>("Customer/GetCustomerInfo");

            return new ResultModel<model.CustomerInfo>(result);
        }
    }
}
