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
        public IBaseRepository _instrumentsRepository { get; set; }
        public ExternalOrderService(IBaseRepository baseRepository) : base(baseRepository)
        {
            _instrumentsRepository = baseRepository;
            
        }

        public AddOrderResult Add(AddOrderModel addOrderModel)
        {
          

            var result = HttpPostRequest<AddOrderResult>("order/add", JsonConvert.SerializeObject(addOrderModel), addOrderModel.Token);
            
            if (result.statusCode != 200)
            {
                //TODO
            }
            return result;
        }
        
        public AllOrders GetAll(OmsBaseModel getAllModel)
        {
            var result = HttpGetRequest<AllOrders>("order/all", getAllModel.Authorization);
            if (result.statusCode != 200)
            {
               //TODO
            }
            return result;
        }


        public ActiveOrdersResult GetAllActives(ViewBaseModel baseModel)
        {
            var result = HttpGetRequest<ActiveOrdersResult>("order/all/active",baseModel.Token);
            if (result.statusCode != 200)
            {
                //TODO
            }
            return result;
        }

        public UpdatedOrder Update(UpdateOrder model)
        {
            var result = HttpPutRequest<UpdatedOrder>($"order/update/{model.InstrumentId}", JsonConvert.SerializeObject(model), model.Authorization);
            if (result.statusCode != 200)
            {
                //TODO
            }
            return result;
        }

        public CanceledOrder Cancel(CancelOrder model)
        {
            var result = HttpDeleteRequest<CanceledOrder>($"order/cancel/{model.InstrumentId}", JsonConvert.SerializeObject(model), model.Authorization);
            if (result.statusCode != 200)
            {
                //TODO
            }
            return result;
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
