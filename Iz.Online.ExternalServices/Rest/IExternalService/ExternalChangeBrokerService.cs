using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iz.Online.ExternalServices.Rest.ExternalService;
using Iz.Online.ExternalServices.Rest.Infrastructure;
using Iz.Online.OmsModels.InputModels.SuperVisory;
using Iz.Online.OmsModels.ResponsModels.SuperVisory;
using Iz.Online.Reopsitory.IRepository;
using Izi.Online.ViewModels.ChangeBroker;
using Izi.Online.ViewModels.Reports;
using Izi.Online.ViewModels.ShareModels;
using Newtonsoft.Json;

namespace Iz.Online.ExternalServices.Rest.IExternalService
{
    public class ExternalChangeBrokerService : BaseService, IExternalChangeBrokerService
    {
        private readonly IExternalChangeBrokerService _externalChangeBrokerService;
        private readonly int APIPORT= 2;
       public ExternalChangeBrokerService(IBaseRepository baseRepository) : base(baseRepository, ServiceProvider.BackOffice)
        {

        }

        public async Task<ResultModel<Requests>> AllRequests(GetAllnput model)
        {
            var result = await HttpPostRequest<Requests>("api/v1/RQ/Requests/GetAll", JsonConvert.SerializeObject(model));
            return new ResultModel<Requests>(result, result.HttpStatusCode == 200, result.Message, result.HttpStatusCode);
        }

        public async Task<ResultModel<DocData>> GetDoc(BaseInput model)
        {
            var result = await  HttpPostRequest<DocData>("api/v1/RQ/Requests/Online/GetDoc", JsonConvert.SerializeObject(model));
            return new ResultModel<DocData>(result, result.HttpStatusCode == 200, result.Message, result.HttpStatusCode);


        }
        public async Task<ResultModel<GetOneData>> GetOne(BaseInput model)
        {
            var result = await HttpPostRequest<GetOneData>("api/v1/RQ/Requests/Online/GetOne", JsonConvert.SerializeObject(model));
            return new  ResultModel<GetOneData>(result, result.HttpStatusCode == 200, result.Message, result.HttpStatusCode);


        }
        public async Task<ResultModel<AddReq>> AddRequest(OmsModels.InputModels.SuperVisory.NewRequest model)
        {
            var result = await HttpPostRequest<AddReq>("api/v1/RQ/Requests/Online/AddOne", JsonConvert.SerializeObject(model));
            return new ResultModel<AddReq>(result, result.HttpStatusCode == 200, result.Message, result.HttpStatusCode);
        }

        public async Task<ResultModel<EditReq>> EditRequest(EditModel model)
        {
           
                var result = await HttpPostRequest<EditReq>("api/v1/RQ/Requests/Online/Edit", JsonConvert.SerializeObject(model));
            return new ResultModel<EditReq>(result, result.HttpStatusCode == 200, result.Message, result.HttpStatusCode);
        }

        public async Task<ResultModel<DeleteReq>> DeleteRequest(BaseInput model)
        {
            var result = await HttpPostRequest<DeleteReq>("api/v1/RQ/Requests/Online/Delete", JsonConvert.SerializeObject(model));
            return new ResultModel<DeleteReq>(result, result.HttpStatusCode == 200, result.Message, result.HttpStatusCode);
        }

        public async Task<ResultModel<RequestHistories>> RequestHistory(BaseInput model)
        {
             var result =  await HttpPostRequest <RequestHistories>("api/v1/RQ/Requests/Online/History", JsonConvert.SerializeObject(model));
            return new ResultModel<RequestHistories>(result, result.HttpStatusCode == 200, result.Message, result.HttpStatusCode);
        }
        public async Task<ResultModel<SuperVisoryReports>> Report(PagingParam<SuperVisoryFilter> model)
        {
            var result = await HttpPostRequest<SuperVisoryReports>("api/v1/RQ/Requests/Online/History", JsonConvert.SerializeObject(model));
            return new ResultModel<SuperVisoryReports>(result, result.HttpStatusCode == 200, result.Message, result.HttpStatusCode);
        }
    }
}
