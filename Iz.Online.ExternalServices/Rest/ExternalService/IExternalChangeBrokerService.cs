using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Izi.Online.ViewModels.ChangeBroker;

namespace Iz.Online.ExternalServices.Rest.ExternalService
{
    public interface IExternalChangeBrokerService
    {
        Task<List<Request>> AllRequests();
        Task<Request> RequestDetails(long requestId);
        Task<byte[]> GetDocument(long documentId);
        Task<long> AddRequest(NewRequest model);
        Task<bool> EditRequest(NewRequest model);
        Task<bool> DeleteRequest(long requestId);
        Task<List<RequestsHistory>> RequestHistory(long requestId);
    }
}
