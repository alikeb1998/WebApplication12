using Iz.Online.ExternalServices.Rest.ExternalService;
using Izi.Online.ViewModels.IO;
using Izi.Online.ViewModels.ShareModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.Services.IServices
{
    public interface IInitialReleaseService
    {
        string Token { get; set; }
        IExternalInitialReleaseService _externalInitialReleaseService { get; }

        Task<ResultModel<List<Offers>>> OpenOffers();
        Task<ResultModel<List<ActiveOrder>>> ActiveOrders();
        Task<ResultModel<bool>> Delete(int id);
    }
}
