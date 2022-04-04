using Iz.Online.ExternalServices.Rest.ExternalService;
using Iz.Online.ExternalServices.Rest.Infrastructure;
using Iz.Online.OmsModels.InputModels;
using Newtonsoft.Json;
using Iz.Online.OmsModels.InputModels.Order;
using Iz.Online.Reopsitory.IRepository;
using Iz.Online.Reopsitory.Repository;
using Iz.Online.OmsModels.ResponsModels.Order;
using Izi.Online.ViewModels.ShareModels;
using AddOrderModel = Izi.Online.ViewModels.Orders.AddOrderModel;
using  Iz.Online.OmsModels.InputModels.Order;

namespace Iz.Online.ExternalServices.Rest.IExternalService
{
    public class ExternalOrderService : BaseService, IExternalOrderService
    {
        private readonly IBaseRepository _instrumentsRepository;

        public ExternalOrderService(IBaseRepository baseRepository) : base(baseRepository, ServiceProvider.Oms)
        {
            _instrumentsRepository = baseRepository;
        }

        public async Task<ResultModel<AddOrderResult>> Add(AddOrderModel addOrderModel)
        {
            var result = await HttpPostRequest<AddOrderResult>("order/add", JsonConvert.SerializeObject(addOrderModel));
            return new ResultModel<AddOrderResult> (result, result.statusCode == 200, result.clientMessage, result.statusCode);
        }
        
        public async Task<ResultModel<AllOrders>> GetAll()
        {
            var result = await HttpGetRequest<AllOrders>("order/all");
            return new ResultModel<AllOrders>(result, result.statusCode == 200, result.clientMessage, result.statusCode);
        }


        public async Task<ResultModel<ActiveOrdersResult>> GetAllActives()
        {
            var result = await HttpGetRequest<ActiveOrdersResult>("order/all/active");
            return new ResultModel<ActiveOrdersResult>(result, result.statusCode == 200, result.clientMessage, result.statusCode);
        }
      

        public async Task<ResultModel<UpdatedOrder>> Update(UpdateOrder model)
        {
            var result = await HttpPutRequest<UpdatedOrder>($"order/update/{model.InstrumentId}", JsonConvert.SerializeObject(model));
            return new ResultModel<UpdatedOrder>(result, result.statusCode == 200, result.clientMessage, result.statusCode);
        }

        public async Task<ResultModel<CanceledOrder>> Cancel(CancelOrder model)
        {
            var result = await  HttpDeleteRequest<CanceledOrder>($"order/cancel/{model.InstrumentId}", JsonConvert.SerializeObject(model));
            return new ResultModel<CanceledOrder>(result, result.statusCode == 200, result.clientMessage, result.statusCode);
        }
    }
}
