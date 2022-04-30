using Iz.Online.OmsModels.ResponsModels;
using Iz.Online.OmsModels.ResponsModels.News;
using Izi.Online.ViewModels.ShareModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.ExternalServices.Rest.ExternalService
{
    public interface IExternalNewsService
    {
        string Token { get; set; }

        Task<ResultModel<ObserverMessages>> Messages();
        Task<ResultModel<OmsResponseBaseModel>> Read(string id);
        Task<ResultModel<UnreadMessages>> UnreadMessages();
    }
}
