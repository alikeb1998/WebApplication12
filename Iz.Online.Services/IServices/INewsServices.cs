using Iz.Online.ExternalServices.Rest.ExternalService;
using Izi.Online.ViewModels.News;
using Izi.Online.ViewModels.ShareModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iz.Online.Services.IServices
{
    public interface INewsServices
    {
        string Token { get; set; }
        IExternalNewsService _externalNewsService { get; }
        Task<ResultModel<List<Message>>> Messages();
        Task<ResultModel<bool>> Read(string id);
        Task<ResultModel<MessageIds>> UnreadMessages();
    }
}
