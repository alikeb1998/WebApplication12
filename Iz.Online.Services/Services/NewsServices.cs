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
                return new ResultModel<List<Message>>(null, res.StatusCode == res.StatusCode, res.Message, res.StatusCode);
            var result = res.Model.Messages.Select(m => new Message()
            {
                Id = m.Id,
                Content = m.Text,
                Date = m.Date,
                Title = m.Title
            }).ToList();
            return new ResultModel<List<Message>>(result);

        }
    }
}
