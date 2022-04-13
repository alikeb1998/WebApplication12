using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iz.Online.OmsModels.InputModels.SuperVisory;
using Iz.Online.OmsModels.ResponsModels.SuperVisory;
using Izi.Online.ViewModels.ChangeBroker;
using Izi.Online.ViewModels.Reports;
using Izi.Online.ViewModels.ShareModels;

namespace Iz.Online.ExternalServices.Rest.ExternalService
{
    public interface IExternalChangeBrokerService
    {
        Task<ResultModel<Requests>> AllRequests(GetAllnput model);
        Task<ResultModel<DocData>> GetDoc(BaseInput model);
        Task<ResultModel<GetOneData>> GetOne(BaseInput model);
        Task<ResultModel<AddReq>> AddRequest(OmsModels.InputModels.SuperVisory.NewRequest model);
        Task<ResultModel<EditReq>> EditRequest(EditModel model);
        Task<ResultModel<DeleteReq>> DeleteRequest(BaseInput model);
        Task<ResultModel<RequestHistories>> RequestHistory(BaseInput model);
        Task<ResultModel<SuperVisoryReports>> Report(PagingParam<SuperVisoryFilter> model);

    }
}
