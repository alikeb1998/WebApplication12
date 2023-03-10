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

namespace Iz.Online.Services.IServices
{
    public interface IChangeBrokerService
    {
        Task<ResultModel<List<Izi.Online.ViewModels.ChangeBroker.Request>>> AllRequests(GetAllnput model);
        Task<ResultModel<DocDto>> GetDoc(BaseInput model);
        Task<ResultModel<GetReq>> GetOne(BaseInput model);
        Task<ResultModel<bool>> AddRequest(OmsModels.InputModels.SuperVisory.NewRequest model);
        Task<ResultModel<bool>> EditRequest(EditModel model);
        Task<ResultModel<bool>> DeleteRequest(EditModel model);
        Task<ResultModel<List<RequestsHistory>>> RequestHistory(BaseInput model);
        Task<ResultModel<SuperVisoryPaged>> Report(PagingParam<SuperVisoryFilter> filter);
    }
}
