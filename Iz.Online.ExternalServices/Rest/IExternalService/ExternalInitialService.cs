using Iz.Online.ExternalServices.Rest.ExternalService;
using Iz.Online.ExternalServices.Rest.Infrastructure;
using Iz.Online.OmsModels.InputModels.IO;
using Iz.Online.OmsModels.ResponsModels;
using Iz.Online.OmsModels.ResponsModels.IO;
using Iz.Online.OmsModels.ResponsModels.Order;
using Iz.Online.Reopsitory.IRepository;
using Izi.Online.ViewModels.ShareModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActiveOrders = Iz.Online.OmsModels.ResponsModels.IO.ActiveOrders;

namespace Iz.Online.ExternalServices.Rest.IExternalService
{
    public class ExternalInitialService : BaseService, IExternalInitialReleaseService
    {
        public ExternalInitialService(IBaseRepository baseRepository, ServiceProvider provider = ServiceProvider.Oms) : base(baseRepository, provider)
        {
        }

        public async Task<ResultModel<IoOrder>> Add(AddIo model)
        {
            var result = await HttpPostRequest<IoOrder>("io/add", JsonConvert.SerializeObject(model));
            return new ResultModel<IoOrder>(result, result.statusCode == 200, result.clientMessage, result.statusCode);
        }       
        public async Task<ResultModel<OpenOffers>> Open()
        {
            var result = await HttpGetRequest<OpenOffers>("order/offer/open");
            return new ResultModel<OpenOffers>(result, result.statusCode == 200, result.clientMessage, result.statusCode);
        }   
        public async Task<ResultModel<ActiveOrders>> Actives()
        {
            var result = await HttpGetRequest<ActiveOrders>("io/active");
            return new ResultModel<ActiveOrders>(result, result.statusCode == 200, result.clientMessage, result.statusCode);
        }

        public async Task<ResultModel<bool>> Delete(int id)
        {
            var result = await HttpGetRequest<OmsResponseBaseModel>($"io/{id}");
            return new ResultModel<bool>(result.statusCode==200, result.statusCode == 200, result.clientMessage, result.statusCode);
        }
    }
}
