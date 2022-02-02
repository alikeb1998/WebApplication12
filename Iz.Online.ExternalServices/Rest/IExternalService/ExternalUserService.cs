using Iz.Online.ExternalServices.Rest.ExternalService;
using Iz.Online.ExternalServices.Rest.Infrastructure;
using Iz.Online.OmsModels.InputModels;
using Iz.Online.OmsModels.ResponsModels.User;
using Iz.Online.Reopsitory.IRepository;


namespace Iz.Online.ExternalServices.Rest.IExternalService
{
    public class ExternalUserService : BaseService, IExternalUserService
    {

        public ExternalUserService(IBaseRepository baseRepository) : base(baseRepository)
        {
        }
        
        public Wallet Wallet(OmsBaseModel model)
        {
            return HttpGetRequest<Wallet>("user/wallet");
        }

    }
}
