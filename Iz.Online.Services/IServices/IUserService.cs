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
        Task<ResultModel<bool>> SetUserHub(string token, string hubId);
        Task<ResultModel<List<Asset>>> AllAssets();
        Task<ResultModel<List<Asset>>> AllAssetsPaged(PortfoFilter filter);
        Task<ResultModel<Wallet>> Wallet();
        List<Izi.Online.ViewModels.AppConfigs> AppConfigs();
        ResultModel<string> GetUserLocalToken(string  omsId);
        Task<ResultModel<Captcha>> Captcha();
        Task<ResultModel<OtpResult>> SendOtp(Credentials credentials);
        Task<ResultModel<CheckedOtp>> CheckOtp(Otp otp);
        Task<ResultModel<bool>> LogOut();
        Task<ResultModel<CustomerData>> GetCustomerInfo();

    }
}
