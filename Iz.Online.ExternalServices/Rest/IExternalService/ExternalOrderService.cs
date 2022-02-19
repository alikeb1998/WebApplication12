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
        public string token { get; set; }
        public IBaseRepository _instrumentsRepository { get; set; }
        public ExternalOrderService(IBaseRepository baseRepository) : base(baseRepository)
        {
            _instrumentsRepository = baseRepository;
        }

        public ResultModel<AddOrderResult> Add(AddOrderModel addOrderModel)
        {
            var result = HttpPostRequest<AddOrderResult>("order/add", JsonConvert.SerializeObject(addOrderModel));
            
            if (result.statusCode != 200)
            {
                return new ResultModel<AddOrderResult>(result);
            }
            return new ResultModel<AddOrderResult>(result, result.statusCode == 200, result.clientMessage, result.statusCode);
        }
        
        public ResultModel<AllOrders> GetAll()
        {
            var result = HttpGetRequest<AllOrders>("order/all");
            if (result.statusCode != 200)
            {
                return new ResultModel<AllOrders>(result);
            }
            return new ResultModel<AllOrders>(result, result.statusCode == 200, result.clientMessage, result.statusCode);
        }


        public ResultModel<ActiveOrdersResult> GetAllActives()
        {
            var result = HttpGetRequest<ActiveOrdersResult>("order/all/active");
            if (result.statusCode != 200)
            {
                return new ResultModel<ActiveOrdersResult>(result);
            }
            return new ResultModel<ActiveOrdersResult>(result, result.statusCode == 200, result.clientMessage, result.statusCode);
        }

        public ResultModel<UpdatedOrder> Update(UpdateOrder model)
        {
            var result = HttpPutRequest<UpdatedOrder>($"order/update/{model.InstrumentId}", JsonConvert.SerializeObject(model));
            if (result.statusCode != 200)
            {
                return new ResultModel<UpdatedOrder>(result);
            }
            return new ResultModel<UpdatedOrder>(result, result.statusCode == 200, result.clientMessage, result.statusCode);
        }

        public ResultModel<CanceledOrder> Cancel(CancelOrder model)
        {
            var result = HttpDeleteRequest<CanceledOrder>($"order/cancel/{model.InstrumentId}", JsonConvert.SerializeObject(model));
            if (result.statusCode != 200)
            {
                return new ResultModel<CanceledOrder>(result);
            }
            return new ResultModel<CanceledOrder>(result, result.statusCode == 200, result.clientMessage, result.statusCode);
        }

       
        //public AssetsList GetAllAssets(ViewBaseModel baseModel)
        //{
        //    var result = HttpGetRequest<AssetsList>("order/asset/all", baseModel.Token);
        //    if (result.statusCode != 200)
        //    {
        //        //TODO
        //    }
        //    return result;
        //}
    }
}
