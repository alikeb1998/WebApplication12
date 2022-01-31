

using Iz.Online.OmsModels.InputModels;
using Iz.Online.OmsModels.ResponsModels.User;

namespace Iz.Online.ExternalServices.Rest.ExternalService
{
    public interface IExternalUserService
    {
        Wallet Wallet(OmsBaseModel model);
    }
}
