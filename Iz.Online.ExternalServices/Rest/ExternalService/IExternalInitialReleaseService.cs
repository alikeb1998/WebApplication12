using Iz.Online.OmsModels.ResponsModels.IO;
using Iz.Online.OmsModels.ResponsModels.Order;
using Izi.Online.ViewModels.ShareModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.ExternalServices.Rest.ExternalService
{
    public interface IExternalInitialReleaseService
    {
        string Token { get; set; }
        Task<ResultModel<OpenOffers>> Open();
        Task<ResultModel<OmsModels.ResponsModels.IO.ActiveOrders>> Actives();
        Task<ResultModel<bool>> Delete(int id);
    }
}
