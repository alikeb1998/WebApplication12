using Confluent.Kafka;
using Iz.Online.ExternalServices.Push.IKafkaPushServices;
using Iz.Online.OmsModels.ResponsModels.Order;
using Iz.Online.SignalR;
//using Iz.Online.SignalR;
using Izi.Online.ViewModels.Instruments;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace Iz.Online.ExternalServices.Push.KafkaPushServices

{
    public class PushService : IPushService
    {
        private readonly IHubContext<CustomersHub> _hubContext;
        private readonly ConsumerConfig _consumerConfig;

        public PushService(IHubContext<CustomersHub> hubContext)
        {
            _hubContext = hubContext;
            _consumerConfig = new ConsumerConfig
            {

                BootstrapServers = "192.168.72.222:9092",
                GroupId = "foo",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

        }



        public async Task ConsumeRefreshInstrumentBestLimit(string InstrumentId)
        {
            //InstrumentId = "IRO1FOLD0001"; // <= sample فولاد

            using (var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build())
            {

                consumer.Subscribe($"{InstrumentId}-bestLimit");
                while (true)
                {
                    var consumeResult = consumer.Consume();
                    var prices =
                        JsonConvert.DeserializeObject<OmsModels.ResponsModels.BestLimits.BestLimit>(consumeResult.Message.Value);
                    _hubContext.Clients.All.SendCoreAsync("OnRefreshInstrumentBestLimit", new object[] { consumeResult.Message.Value, InstrumentId, " " });
                }
                consumer.Close();
            }

        }

        public async Task PushOrderAdded(List<string> CustomerHubsId, Izi.Online.ViewModels.Orders.ActiveOrder model)
        {
            //_hubContext.Clients.Users(CustomerHubsId).SendCoreAsync("OnRefreshOrders", new object[] { model});
            _hubContext.Clients.All.SendCoreAsync("OnRefreshOrders", new object[] { model });

        }
    }
}
