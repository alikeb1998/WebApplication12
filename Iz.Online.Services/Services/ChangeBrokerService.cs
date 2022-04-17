using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iz.Online.ExternalServices.Rest.ExternalService;
using Iz.Online.OmsModels.InputModels.SuperVisory;
using Iz.Online.Reopsitory.IRepository;
using Iz.Online.Services.IServices;
using Izi.Online.ViewModels.ChangeBroker;
using Izi.Online.ViewModels.Reports;
using Izi.Online.ViewModels.ShareModels;

namespace Iz.Online.Services.Services
{
    public class ChangeBrokerService : IChangeBrokerService
    {
        public IExternalChangeBrokerService _externalService { get; }
        private readonly ICacheService _cacheService;
        public ChangeBrokerService(IExternalChangeBrokerService externalChangeBrokerService, ICacheService cacheService)
        {
            _externalService = externalChangeBrokerService;
            _cacheService = cacheService;
        }

        public async Task<ResultModel<List<Request>>> AllRequests(GetAllnput model)
        {

            var requests = await _externalService.AllRequests(model);
            var res = requests.Model.Data
                  .Select(x =>
                  new Request()
                  {
                      CreatedAt = x.CreatedAt,
                      CustomerName = x.CustomerName,
                      HasStockSheet = x.HasStockSheet,
                      InstrumentName = x.InstrumentName,
                      LastDescription = x.LastDescription,
                      LastStatus = x.LastStatus,
                      RequestId = x.RequestId
                  }).ToList();


            return new ResultModel<List<Request>>(res);
        }

        public async Task<ResultModel<DocDto>> GetDoc(BaseInput model)
        {
            var doc = await _externalService.GetDoc(model);
            var res = new DocDto() { Document = doc.Model.Data.Document, DocumentExtension = doc.Model.Data.DocumentExtention, DocumentName = doc.Model.Data.DocumentName };
            return new ResultModel<DocDto>(res);
        }

        public async Task<ResultModel<GetReq>> GetOne(BaseInput model)
        {
            var req = await _externalService.GetOne(model);
            var res = new GetReq()
            {
                BranchId = req.Model.Data.BranchId,
                BranchName = req.Model.Data.BranchName,
                Description = req.Model.Data.Description,
                Document = req.Model.Data.Document,
                DocumentExtension = req.Model.Data.DocumentExtension,
                DocumentsName = req.Model.Data.DocumentsName,
                HasStockSheet = req.Model.Data.HasStockSheet,
                InstrumentId = req.Model.Data.InstrumentId,
                InstrumentName = req.Model.Data.InstrumentName,
            };
            return new ResultModel<GetReq>(res);
        }

        public async Task<ResultModel<bool>> AddRequest(OmsModels.InputModels.SuperVisory.NewRequest model)
        {
            var doc = await _externalService.AddRequest(model);
            var res = doc.Model.IsSuccess;
            return new ResultModel<bool>(res);
        }

        public async Task<ResultModel<bool>> EditRequest(EditModel model)
        {
            var req = await _externalService.EditRequest(model);
            var res = req.Model.IsSuccess;
            return new ResultModel<bool>(res);
        }

        public async Task<ResultModel<bool>> DeleteRequest(BaseInput model)
        {
            var req = await _externalService.DeleteRequest(model);

            return new ResultModel<bool>(req.Model.IsSuccess);
        }

        public async Task<ResultModel<List<RequestsHistory>>> RequestHistory(BaseInput model)
        {
            var req = await _externalService.RequestHistory(model);
            var res = req.Model.Data.Select(history => new RequestsHistory()
            {
               
                ChangeAt = history.ChangeAt,
                Description = history.Description,
                Status = history.Status,
                UserName = history.UserName
            
        }).ToList();



            return new ResultModel<List<RequestsHistory>>(res);
        }

        public async Task<ResultModel<List<SuperVisoryReport>>> Report(PagingParam<SuperVisoryFilter> filter)
        {
            var req = await _externalService.Report(filter);
           var res = req.Model.Data.Select(history => new SuperVisoryReport()
            {

                ChangeAt = history.ChangeAt,
                Description = history.Description,
                Status = history.Status,
                UserName = history.UserName,
                CreatedAt = history.CreatedAt,
                Document = history.Document,
                InstrumentId = history.InstrumentId,
                InstrumentName = history.InstrumentName,
                RequestId = history.RequestId,
                StatusText = history.StatusText,
                DocumentExtension = history.DocumentExtension,
                DocumentName = history.DocumentName,

            }).ToList();



            return new ResultModel<List<SuperVisoryReport>>(res);
        }
    }
}
