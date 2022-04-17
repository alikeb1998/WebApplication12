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
    }
}
