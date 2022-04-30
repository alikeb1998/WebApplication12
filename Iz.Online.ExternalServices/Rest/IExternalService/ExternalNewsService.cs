using Iz.Online.OmsModels.ResponsModels.News;
using Izi.Online.ViewModels.ShareModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iz.Online.ExternalServices.Rest.Infrastructure;
using Iz.Online.ExternalServices.Rest.ExternalService;
using Iz.Online.Reopsitory.IRepository;
using Iz.Online.OmsModels.ResponsModels;

namespace Iz.Online.ExternalServices.Rest.IExternalService
{
    public class ExternalNewsService : BaseService, IExternalNewsService
    {
        public ExternalNewsService(IBaseRepository baseRepository, ServiceProvider provider = ServiceProvider.Oms) : base(baseRepository, provider)
        {
        }


        public async Task<ResultModel<ObserverMessages>> Messages()
        {

            var result = await  HttpGetRequest<ObserverMessages>($"rlc/observer-messages");
            return new ResultModel<ObserverMessages>(result, result.statusCode == 200, result.clientMessage, result.statusCode);

        }

        public async Task<ResultModel<OmsResponseBaseModel>> Read(string id)
        {
            var result = await HttpGetRequest<OmsResponseBaseModel>($"rlc/observer/read/{id}");
            return new ResultModel<OmsResponseBaseModel>(result, result.statusCode == 200, result.clientMessage, result.statusCode);
        }

        public async Task<ResultModel<UnreadMessages>> UnreadMessages()
        {
            var result = await HttpGetRequest<UnreadMessages>($"rlc/observer/unread/ids");
            return new ResultModel<UnreadMessages>(result, result.statusCode == 200, result.clientMessage, result.statusCode);
        }
    }
}
