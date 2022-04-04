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

        public async Task<List<Request>> AllRequests()
        {
           return await HttpGetRequest<List<Request>>("");
        }

        public async Task<Request> RequestDetails(long requestId)
        {
            return await HttpGetRequest<Request>("");
        }

        public async Task<byte[]> GetDocument(long documentId)
        {
            return await HttpGetRequest<byte[]>("");
        }

        public async Task<long> AddRequest(NewRequest model)
        {
            return await HttpGetRequest<long>("");
        }

        public async Task<bool> EditRequest(NewRequest model)
        {
            return await HttpGetRequest<bool>("");
        }

        public async Task<bool> DeleteRequest(long requestId)
        {
            return await HttpGetRequest<bool>("");
        }

        public async Task<List<RequestsHistory>> RequestHistory(long requestId)
        {
            return await HttpGetRequest<List<RequestsHistory>>("");
        }
    }
}
