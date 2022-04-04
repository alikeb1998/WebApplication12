using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iz.Online.ExternalServices.Rest.ExternalService;
using Iz.Online.Reopsitory.IRepository;
using Iz.Online.Services.IServices;
using Izi.Online.ViewModels.ChangeBroker;

namespace Iz.Online.Services.Services
{
    //public class ChangeBrokerService : IChangeBrokerService
    //{
    //    public IExternalChangeBrokerService _externalService { get; }
    //    private readonly ICacheService _cacheService;
    //    public ChangeBrokerService(IExternalChangeBrokerService externalChangeBrokerService, ICacheService cacheService)
    //    {
    //        _externalService = externalChangeBrokerService;
    //        _cacheService = cacheService;
    //    }


    //    public async Task<ResultModel<List<Request>>> AllRequests()
    //    {
    //        return await _externalService.AllRequests();
    //    }
        
    //    public async Task<ResultModel<Request>> RequestDetails(long requestId)
    //    {
    //        return await _externalService.RequestDetails(requestId);
    //    }

    //    public async Task<ResultModel<byte[]>> GetDocument(long documentId)
    //    {
    //        return await _externalService.GetDocument(documentId);
    //    }

    //    public async Task<ResultModel<long>> AddRequest(NewRequest model)
    //    {
    //        return await _externalService.AddRequest(model);
    //    }

    //    public async Task<ResultModel<bool>> EditRequest(NewRequest model)
    //    {
    //        return await _externalService.EditRequest(model);
    //    }

    //    public async Task<ResultModel<bool>> DeleteRequest(long requestId)
    //    {
    //        return await _externalService.DeleteRequest(requestId);
    //    }

    //    public async Task<ResultModel<List<RequestsHistory>>> RequestHistory(long requestId)
    //    {
    //        return await _externalService.RequestHistory(requestId);
    //    }
    //}
}
