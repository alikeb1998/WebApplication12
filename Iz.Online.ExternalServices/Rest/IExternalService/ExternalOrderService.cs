using Iz.Online.ExternalServices.Rest.ExternalService;
using Iz.Online.ExternalServices.Rest.Infrastructure;
using Newtonsoft.Json;
using Iz.Online.OmsModels.InputModels.Order;
using Iz.Online.Reopsitory.IRepository;
using Iz.Online.Reopsitory.Repository;
using Iz.Online.OmsModels.ResponsModels.Order;

namespace Iz.Online.ExternalServices.Rest.IExternalService
{
    public class ExternalOrderService : BaseService, IExternalOrderService
    {
        public IBaseRepository _instrumentsRepository { get; set; }
        public ExternalOrderService(IBaseRepository baseRepository) : base(baseRepository)
        {
            _instrumentsRepository = baseRepository;

        }

        public AddOrderResult Add(AddOrder addOrderModel)
        {
            var result = HttpPostRequest<AddOrderResult>("order/add", JsonConvert.SerializeObject(addOrderModel));
            
            if (result.statusCode != 200)
            {
                //TODO
            }
            return result;
        }



    }
}
