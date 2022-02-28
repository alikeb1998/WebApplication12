using Confluent.Kafka;
using Iz.Online.HubConnectionHandler.IServices;
using Iz.Online.HubHandler.IServices;
using Iz.Online.OmsModels.ResponsModels.BestLimits;
using Iz.Online.Reopsitory.IRepository;
using Iz.Online.SignalR;
using Izi.Online.ViewModels.Users;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace Iz.Online.HubHandler.Services
{
    public class HubUserService : IServices.IHubUserService
    {
        private readonly IHubConnationService _hubConnationService;
        private readonly IHubContext<CustomersHub> _hubContext;
        private readonly ConsumerConfig _consumerConfig;

        public HubUserService(IHubConnationService hubConnationService, IHubContext<CustomersHub> hubContext)
        {

            _hubConnationService = hubConnationService;
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
            var c = 0;
            while (c < 50)
            {
                c++;
                try
                {
                    Thread.Sleep(2000);
                    Random rnd = new Random();


                    OmsModels.ResponsModels.BestLimits.BestLimit model = new OmsModels.ResponsModels.BestLimits.BestLimit()
                    {
                        changeRow1 = rnd.Next(20, 30) > 25,
                        changeRow2 = rnd.Next(20, 30) > 25,
                        changeRow3 = rnd.Next(20, 30) > 25,
                        changeRow4 = rnd.Next(20, 30) > 25,
                        changeRow5 = rnd.Next(20, 30) > 25,
                        changeRow6 = rnd.Next(20, 30) > 25,
                        orderRow1 = new OrderRow()
                        {
                            countBestBuy = rnd.Next(20, 50),
                            priceBestBuy = rnd.Next(3000, 50000),
                            volumeBestBuy = rnd.Next(100000, 1000000),
                            countBestSale = rnd.Next(20, 50),
                            priceBestSale = rnd.Next(3000, 50000),
                            volumeBestSale = rnd.Next(100000, 1000000),

                        },
                        orderRow2 = new OrderRow()
                        {
                            countBestBuy = rnd.Next(20, 50),
                            priceBestBuy = rnd.Next(3000, 50000),
                            volumeBestBuy = rnd.Next(100000, 1000000),
                            countBestSale = rnd.Next(20, 50),
                            priceBestSale = rnd.Next(3000, 50000),
                            volumeBestSale = rnd.Next(100000, 1000000),

                        },
                        orderRow3 = new OrderRow()
                        {
                            countBestBuy = rnd.Next(20, 50),
                            priceBestBuy = rnd.Next(3000, 50000),
                            volumeBestBuy = rnd.Next(100000, 1000000),
                            countBestSale = rnd.Next(20, 50),
                            priceBestSale = rnd.Next(3000, 50000),
                            volumeBestSale = rnd.Next(100000, 1000000),

                        },
                        orderRow4 = new OrderRow()
                        {
                            countBestBuy = rnd.Next(20, 50),
                            priceBestBuy = rnd.Next(3000, 50000),
                            volumeBestBuy = rnd.Next(100000, 1000000),
                            countBestSale = rnd.Next(20, 50),
                            priceBestSale = rnd.Next(3000, 50000),
                            volumeBestSale = rnd.Next(100000, 1000000),

                        },
                        orderRow5 = new OrderRow()
                        {
                            countBestBuy = rnd.Next(20, 50),
                            priceBestBuy = rnd.Next(3000, 50000),
                            volumeBestBuy = rnd.Next(100000, 1000000),
                            countBestSale = rnd.Next(20, 50),
                            priceBestSale = rnd.Next(3000, 50000),
                            volumeBestSale = rnd.Next(100000, 1000000),

                        },
                        orderRow6 = new OrderRow()
                        {
                            countBestBuy = rnd.Next(20, 50),
                            priceBestBuy = rnd.Next(3000, 50000),
                            volumeBestBuy = rnd.Next(100000, 1000000),
                            countBestSale = rnd.Next(20, 50),
                            priceBestSale = rnd.Next(3000, 50000),
                            volumeBestSale = rnd.Next(100000, 1000000),

                        }

                    };

                    var prices = JsonConvert.SerializeObject(model);

                    var hubs = _hubConnationService.UserHubsList("user1");
                    if (hubs != null)

                        _hubContext.Clients.Clients(hubs.Select(x => x.HubId)).SendCoreAsync("OnRefreshInstrumentBestLimit", new object[] { prices, $"InstrumentId : '{InstrumentId}{c}'", " " });

                    _hubContext.Clients.All.SendCoreAsync("OnRefreshInstrumentBestLimit", new object[] { prices, $"InstrumentId : '{InstrumentId}22'", " " });
                }
                catch (Exception e)
                {

                }
            }


        }

        public async Task ConsumeRefreshInstrumentBestLimit_Orginal(string InstrumentId)
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

                    //var hubs = _hubConnationService.UserHubsList("user1").HubId;
                    //await _hubContext.Clients.Clients(hubs).SendCoreAsync("OnRefreshInstrumentBestLimit", new object[] { consumeResult.Message.Value, InstrumentId, " " });

                    //_hubContext.Clients.All.SendCoreAsync("OnRefreshInstrumentBestLimit", new object[] { consumeResult.Message.Value, InstrumentId, " " });
                }
                consumer.Close();
            }

        }

        public async Task PushOrderAdded(List<string> CustomerHubsId, Izi.Online.ViewModels.Orders.ActiveOrder model)
        {
            //_hubContext.Clients.Users(CustomerHubsId).SendCoreAsync("OnRefreshOrders", new object[] { model});
            _hubContext.Clients.All.SendCoreAsync("OnRefreshOrders", new object[] { model });

        }

        public Task PushOrderState()
        {


            using (var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build())
            {

                consumer.Subscribe($"OrderTrade");
                while (true)
                {
                    try
                    {
                        var consumeResult = consumer.Consume();
                        var t = consumeResult.Message.Value;
                        _hubContext.Clients.All.SendCoreAsync("OnChangeOrderState", new object[] { consumeResult.Message.Value });

                    }
                    catch (Exception e)
                    {

                    }
                }
                consumer.Close();
            }


        }

        public Task PushTradeState()
        {

            using (var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build())
            {

                consumer.Subscribe($"OrderChange");
                while (true)
                {
                    try
                    {
                        var consumeResult = consumer.Consume();
                        var t = consumeResult.Message.Value;
                        _hubContext.Clients.All.SendCoreAsync("OnChangeTrades", new object[] { consumeResult.Message.Value });

                    }
                    catch (Exception e)
                    {


                    }
                }
                consumer.Close();
            }


        }

        public async Task CreateAllConsumers()
        {

            Task.Run(async () => PushOrderState());
            Task.Run(async () => PushTradeState());


        }
    }
}
