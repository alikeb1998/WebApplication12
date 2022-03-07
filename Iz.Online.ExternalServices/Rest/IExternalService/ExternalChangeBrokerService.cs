using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iz.Online.ExternalServices.Rest.ExternalService;
using Iz.Online.ExternalServices.Rest.Infrastructure;
using Iz.Online.Reopsitory.IRepository;
using Izi.Online.ViewModels.ChangeBroker;

namespace Iz.Online.ExternalServices.Rest.IExternalService
{
    public class ExternalChangeBrokerService : BaseService, IExternalChangeBrokerService
    {
        public ExternalChangeBrokerService(IBaseRepository baseRepository) : base(baseRepository, ServiceProvider.BackOffice)
        {

        }

        public List<Request> AllRequests()
        {
           return HttpGetRequest<List<Request>>("");
        }

        public Request RequestDetails(long requestId)
        {
            return HttpGetRequest<Request>("");
        }

        public byte[] GetDocument(long documentId)
        {
            return HttpGetRequest<byte[]>("");
        }

        public long AddRequest(NewRequest model)
        {
            return HttpGetRequest<long>("");
        }

        public bool EditRequest(NewRequest model)
        {
            return HttpGetRequest<bool>("");
        }

        public bool DeleteRequest(long requestId)
        {
            return HttpGetRequest<bool>("");
        }

        public List<RequestsHistory> RequestHistory(long requestId)
        {
            return HttpGetRequest<List<RequestsHistory>>("");
        }
    }
}
