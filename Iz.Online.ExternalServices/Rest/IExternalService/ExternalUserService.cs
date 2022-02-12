using Iz.Online.ExternalServices.Rest.ExternalService;
using Iz.Online.ExternalServices.Rest.Infrastructure;
using Iz.Online.OmsModels.InputModels;
using Iz.Online.OmsModels.ResponsModels.Order;
using Iz.Online.OmsModels.ResponsModels.User;
using Iz.Online.Reopsitory.IRepository;
using Izi.Online.ViewModels.ShareModels;
using model = Iz.Online.OmsModels.ResponsModels.User;


namespace Iz.Online.ExternalServices.Rest.IExternalService
{
    public class ExternalUserService : BaseService, IExternalUserService
    {

        public ExternalUserService(IBaseRepository baseRepository) : base(baseRepository)
        {
        }

        public ResultModel<Wallet> Wallet(OmsBaseModel model)
        {
            var result = HttpGetRequest<Wallet>("user/wallet", model.Authorization);

            if (result.statusCode != 200)
                return new ResultModel<Wallet>(result, false, result.clientMessage, result.statusCode);
            return new ResultModel<Wallet>(result);
        }

        public ResultModel<AssetsList> GetAllAssets(ViewBaseModel baseModel)
        {
            var result = HttpGetRequest<AssetsList>("order/asset/all", baseModel.Token);
         
            if (result.statusCode != 200)
                return new ResultModel<AssetsList>(result, false, result.clientMessage, result.statusCode);
            return new ResultModel<AssetsList>(result);
        }

    }
}
