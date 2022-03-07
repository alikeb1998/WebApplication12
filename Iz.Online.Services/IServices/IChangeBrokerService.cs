using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Izi.Online.ViewModels.ChangeBroker;

namespace Iz.Online.Services.IServices
{
    public  interface  IChangeBrokerService
    {
        List<Request> AllRequests();
        Request RequestDetails(long requestId);
        byte[] GetDocument(long documentId);
        long AddRequest(NewRequest model);
        bool EditRequest(NewRequest model);
        bool DeleteRequest(long requestId);
        List<RequestsHistory> RequestHistory(long requestId);
    }
}
