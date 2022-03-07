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
    public class ChangeBrokerService : IChangeBrokerService
    {
        public IExternalChangeBrokerService _externalService { get; }
        private readonly ICacheService _cacheService;
        public ChangeBrokerService(IExternalChangeBrokerService externalChangeBrokerService, ICacheService cacheService)
        {
            _externalService = externalChangeBrokerService;
            _cacheService = cacheService;
        }


        public List<Request> AllRequests()
        {
            return _externalService.AllRequests();
        }

        public Request RequestDetails(long requestId)
        {
            return _externalService.RequestDetails(requestId);
        }

        public byte[] GetDocument(long documentId)
        {
            return _externalService.GetDocument(documentId);
        }

        public long AddRequest(NewRequest model)
        {
            return _externalService.AddRequest(model);
        }

        public bool EditRequest(NewRequest model)
        {
            return _externalService.EditRequest(model);
        }

        public bool DeleteRequest(long requestId)
        {
            return _externalService.DeleteRequest(requestId);
        }

        public List<RequestsHistory> RequestHistory(long requestId)
        {
            return _externalService.RequestHistory(requestId);
        }
    }
}
