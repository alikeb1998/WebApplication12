using Confluent.Kafka;
using Iz.Online.ExternalServices.Push.IKafkaPushServices;
using Iz.Online.SignalR;
//using Iz.Online.SignalR;
using Izi.Online.ViewModels.Instruments;
using Microsoft.AspNetCore.SignalR;

namespace Iz.Online.ExternalServices.Push.KafkaPushServices

{
    public class PushService : IPushService
    {
        public IHubContext<CustomersHub> _hubContext;

        public PushService(IHubContext<CustomersHub> hubContext)
        {
            _hubContext = hubContext;
        }



        public Task<List<InstrumentsDetails_Delete>> OnRefreshInstrumentDetails()
        {
            _hubContext.Clients.All.SendCoreAsync("ReceiveMessage", new object[] {"asa" , "As"});
            var config = new ConsumerConfig
            {
                
                BootstrapServers = "192.168.72.222:9092",
                GroupId = "foo",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };



            using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                var instrumentId = "";

                consumer.Subscribe($"{instrumentId}-bestLimit");
                
                var consumeResult = consumer.Consume();

                var tt  = consumeResult.Message.Value;
                // handle consumed message.

                consumer.Close();
            }



            return null;

        }
    }
}
