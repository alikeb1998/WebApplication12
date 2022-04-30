using Iz.Online.ExternalServices.Rest.ExternalService;
using Iz.Online.Services.IServices;
using Izi.Online.ViewModels.News;
using Izi.Online.ViewModels.ShareModels;


namespace Iz.Online.Services.Services
{
    public class NewsServices : INewsServices
    {
        public string Token { get; set; }
        public IExternalNewsService _externalNewsService { get; }
        public NewsServices(IExternalNewsService externalNewsService)
        {
            _externalNewsService = externalNewsService;
        }


        public async Task<ResultModel<List<Message>>> Messages()
        {
            var res = await _externalNewsService.Messages();

            if (!res.IsSuccess || res.Model.message == null)
                return new ResultModel<List<Message>>(null, res.StatusCode == 200, res.Message, res.StatusCode);
            var result = res.Model.Messages.Select(m => new Message()
            {
                Id = m.Id,
                Content = m.Text,
                Date = m.Date,
                Title = m.Title
            }).ToList();
            return new ResultModel<List<Message>>(result);

        }

        public async Task<ResultModel<bool>> Read(string id)
        {
            var res = await _externalNewsService.Read(id);

            if (!res.IsSuccess || res.Model.message == null)
                return new ResultModel<bool>(false, res.StatusCode == 200, res.Message, res.StatusCode);
          
            return new ResultModel<bool>(res.StatusCode==200);
          
        }

        public async Task<ResultModel<MessageIds>> UnreadMessages()
        {
            var res = await _externalNewsService.UnreadMessages();

            if (!res.IsSuccess || res.Model.message == null)
                return new ResultModel<MessageIds>(null, res.StatusCode == 200, res.Message, res.StatusCode);
            var result = new MessageIds()
            {
                Messages = res.Model.Ids
            };
            return new ResultModel<MessageIds>(result);

        }
    }
}
