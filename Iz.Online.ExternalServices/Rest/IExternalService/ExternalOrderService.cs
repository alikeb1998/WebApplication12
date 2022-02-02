using Iz.Online.ExternalServices.Rest.ExternalService;
using Iz.Online.ExternalServices.Rest.Infrastructure;
using Iz.Online.OmsModels.InputModels;
using Newtonsoft.Json;
using Iz.Online.OmsModels.InputModels.Order;
using Iz.Online.Reopsitory.IRepository;
using Iz.Online.Reopsitory.Repository;
using Iz.Online.OmsModels.ResponsModels.Order;
using Izi.Online.ViewModels.ShareModels;

namespace Iz.Online.ExternalServices.Rest.IExternalService
{
    public class ExternalOrderService : BaseService, IExternalOrderService
    {
        public IBaseRepository _instrumentsRepository { get; set; }
        public ExternalOrderService(IBaseRepository baseRepository) : base(baseRepository)
        {
            _instrumentsRepository = baseRepository;
            
        }

        public AddOrderResult Add(AddOrder addOrderModel, string token)
        {
          

            var result = HttpPostRequest<AddOrderResult>("order/add", JsonConvert.SerializeObject(addOrderModel), token);
            
            if (result.statusCode != 200)
            {
                //TODO
            }
            return result;
        }
        
        public AllOrders GetAll(OmsBaseModel getAllModel, string token)
        {
            var result = HttpGetRequest<AllOrders>("order/all", token);
            if (result.statusCode != 200)
            {
               //TODO
            }
            return result;
        }


        public ActiveOrdersResult GetAllActives(ViewBaseModel baseModel, string token)
        {
            var result = HttpGetRequest<ActiveOrdersResult>("order/all/active", token);
            if (result.statusCode != 200)
            {
                //TODO
            }
            return result;
        }
    }
}
