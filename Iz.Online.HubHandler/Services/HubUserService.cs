using Confluent.Kafka;
using Iz.Online.HubConnectionHandler.IServices;
using Iz.Online.HubHandler.IServices;
using Iz.Online.OmsModels.ResponsModels.BestLimits;
using Iz.Online.Reopsitory.IRepository;
using Iz.Online.SignalR;
using Izi.Online.ViewModels.Users;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using OrderChanged = Iz.Online.OmsModels.ResponsModels.Order.OrderChangeTopic;

namespace Iz.Online.HubHandler.Services
{

    //topics =>
    //OrderChange (active order tab ) ,\
    //OrderTrade  (today trades tab) , \
    //{InstrumentId}-bestLimit ,       \
    //CustomerWallet ,                 \
    //CustomerPortfolioL  ,            \
    //{InstrumentId}-price (details)   \

    public class HubUserService : IServices.IHubUserService
    {
        private readonly IHubConnationService _hubConnationService;
        private readonly IHubContext<CustomersHub> _hubContext;
        private readonly ConsumerConfig _consumerConfig;
        private static bool ConsumerIsStar = false;
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
                //Task.Run(async () => CreateAllConsumers());
                c++;
                try
                {
                    Thread.Sleep(2000);
                    Random rnd = new Random();


                    Izi.Online.ViewModels.Instruments.BestLimit.BestLimits model = new Izi.Online.ViewModels.Instruments.BestLimit.BestLimits()
                    {

                        orderRow1 = new Izi.Online.ViewModels.Instruments.BestLimit.OrderRow()
                        {
                            countBestBuy = rnd.Next(20, 50),
                            priceBestBuy = rnd.Next(3000, 50000),
                            volumeBestBuy = rnd.Next(100000, 1000000),
                            countBestSale = rnd.Next(20, 50),
                            priceBestSale = rnd.Next(3000, 50000),
                            volumeBestSale = rnd.Next(100000, 1000000),

                        },
                        orderRow2 = new Izi.Online.ViewModels.Instruments.BestLimit.OrderRow()
                        {
                            countBestBuy = rnd.Next(20, 50),
                            priceBestBuy = rnd.Next(3000, 50000),
                            volumeBestBuy = rnd.Next(100000, 1000000),
                            countBestSale = rnd.Next(20, 50),
                            priceBestSale = rnd.Next(3000, 50000),
                            volumeBestSale = rnd.Next(100000, 1000000),

                        },
                        orderRow3 = new Izi.Online.ViewModels.Instruments.BestLimit.OrderRow()
                        {
                            countBestBuy = rnd.Next(20, 50),
                            priceBestBuy = rnd.Next(3000, 50000),
                            volumeBestBuy = rnd.Next(100000, 1000000),
                            countBestSale = rnd.Next(20, 50),
                            priceBestSale = rnd.Next(3000, 50000),
                            volumeBestSale = rnd.Next(100000, 1000000),

                        },
                        orderRow4 = new Izi.Online.ViewModels.Instruments.BestLimit.OrderRow()
                        {
                            countBestBuy = rnd.Next(20, 50),
                            priceBestBuy = rnd.Next(3000, 50000),
                            volumeBestBuy = rnd.Next(100000, 1000000),
                            countBestSale = rnd.Next(20, 50),
                            priceBestSale = rnd.Next(3000, 50000),
                            volumeBestSale = rnd.Next(100000, 1000000),

                        },
                        orderRow5 = new Izi.Online.ViewModels.Instruments.BestLimit.OrderRow()
                        {
                            countBestBuy = rnd.Next(20, 50),
                            priceBestBuy = rnd.Next(3000, 50000),
                            volumeBestBuy = rnd.Next(100000, 1000000),
                            countBestSale = rnd.Next(20, 50),
                            priceBestSale = rnd.Next(3000, 50000),
                            volumeBestSale = rnd.Next(100000, 1000000),

                        },
                        orderRow6 = new Izi.Online.ViewModels.Instruments.BestLimit.OrderRow()
                        {
                            countBestBuy = rnd.Next(20, 50),
                            priceBestBuy = rnd.Next(3000, 50000),
                            volumeBestBuy = rnd.Next(100000, 1000000),
                            countBestSale = rnd.Next(20, 50),
                            priceBestSale = rnd.Next(3000, 50000),
                            volumeBestSale = rnd.Next(100000, 1000000),

                        },


                    };

                    var prices = JsonConvert.SerializeObject(model);

                    var hubs = _hubConnationService.UserHubsList("KafkaUserId");
                    if (hubs != null)
                        _hubContext.Clients.Clients(hubs.Hubs).SendCoreAsync("OnRefreshInstrumentBestLimit", new object[] { prices, $"InstrumentId : '{InstrumentId}{c}'", " " });

                    _hubContext.Clients.All.SendCoreAsync("OnRefreshInstrumentBestLimit", new object[] { prices, $"InstrumentId : '{InstrumentId}'", " " });
                }
                catch (Exception e)
                {

                }
            }


        }

        /// <summary>
        /// {InstrumentId}-bestLimit
        /// </summary>
        public async Task ConsumeRefreshInstrumentBestLimit_Orginal(string InstrumentId)
        {
            //InstrumentId = "IRO1FOLD0001"; // <= sample فولاد

            using (var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build())
            {

                consumer.Subscribe($"{InstrumentId}-bestLimit");
                while (true)
                {
                    //Izi.Online.ViewModels.Instruments.BestLimit.BestLimits
                   var consumeResult = consumer.Consume();
                    var prices =
                        JsonConvert.DeserializeObject<OmsModels.ResponsModels.BestLimits.BestLimit>(consumeResult.Message.Value);
                    //TODO cast to Izi.Online.ViewModels.Instruments.BestLimit.BestLimits

                    //var hubs = _hubConnationService.UserHubsList("user1");
                    //await _hubContext.Clients.Clients(hubs.Hubs).SendCoreAsync("OnRefreshInstrumentBestLimit", new object[] { consumeResult.Message.Value, InstrumentId, " " });

                    _hubContext.Clients.All.SendCoreAsync("OnRefreshInstrumentBestLimit", new object[] { consumeResult.Message.Value, InstrumentId, " " });
                }
                consumer.Close();
            }

        }

        /// <summary>
        ///  topic=> {InstrumentId}-price
        /// </summary>
        /// <returns></returns>
        public async Task PushPrice(string InstrumentId)
        {
            using (var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build())
            {

                var c = 0;
                consumer.Subscribe($"{InstrumentId}-price");
                while (true)
                {
                    try
                    {
                        var hubs = _hubConnationService.UserHubsList("CustomerInfoUserId");
                        var consumeResult = consumer.Consume();
                        var model1 = JsonConvert.DeserializeObject<object>(consumeResult.Message.Value);
                        //var hubs = _hubConnationService.UserHubsList(model1.Customer);

                        if (hubs != null)
                            _hubContext.Clients.Clients(hubs.Hubs).SendCoreAsync($"{model1}-price", new object[] { model1, $"InstrumentId : '{model1}{c}'", " " });

                        _hubContext.Clients.All.SendCoreAsync($"{model1}-price", new object[] { consumeResult.Message.Value });



                        var t = consumeResult.Message.Value;
                    }
                    catch (Exception e)
                    {


                    }
                }
                consumer.Close();
            }



        }



        //// start auto


        /// <summary>
        /// topic => OrderTrade
        /// </summary>
        public async Task PushOrderAdded()
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
                        _hubContext.Clients.All.SendCoreAsync("OnOrderAdded", new object[] { consumeResult.Message.Value });

                    }
                    catch (Exception e)
                    {

                    }
                }
                consumer.Close();
            }

        }

        /// <summary>
        /// topic=> OrderChange
        /// </summary>
        public async Task PushTradeState()
        {

            using (var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build())
            {
                var c = 0;
                consumer.Subscribe($"OrderChange");
                while (true)
                {
                    try
                    {
                        var hubs = _hubConnationService.UserHubsList("CustomerInfoUserId");
                        var consumeResult = consumer.Consume();
                        var model1 = JsonConvert.DeserializeObject<OrderChanged>(consumeResult.Message.Value);
                        //var hubs = _hubConnationService.UserHubsList(model1.Customer);

                        if (hubs != null)
                            _hubContext.Clients.Clients(hubs.Hubs).SendCoreAsync("OnChangeTrades", new object[] { model1.Price, $"InstrumentId : '{model1.Instrument}{c}'", " " });

                        _hubContext.Clients.All.SendCoreAsync("OnChangeTrades", new object[] { consumeResult.Message.Value });

                        #region fake data

                        //while (c < 50)
                        //{
                        //    c++;
                        //    try
                        //    {
                        //        Random rnd = new Random();
                        //        Thread.Sleep(2000);

                        //        OmsModels.ResponsModels.BestLimits.BestLimit model = new OmsModels.ResponsModels.BestLimits.BestLimit()
                        //        {
                        //            changeRow1 = rnd.Next(20, 30) > 25,
                        //            changeRow2 = rnd.Next(20, 30) > 25,
                        //            changeRow3 = rnd.Next(20, 30) > 25,
                        //            changeRow4 = rnd.Next(20, 30) > 25,
                        //            changeRow5 = rnd.Next(20, 30) > 25,
                        //            changeRow6 = rnd.Next(20, 30) > 25,
                        //            orderRow1 = new OrderRow()
                        //            {
                        //                countBestBuy = rnd.Next(20, 50),
                        //                priceBestBuy = rnd.Next(3000, 50000),
                        //                volumeBestBuy = rnd.Next(100000, 1000000),
                        //                countBestSale = rnd.Next(20, 50),
                        //                priceBestSale = rnd.Next(3000, 50000),
                        //                volumeBestSale = rnd.Next(100000, 1000000),

                        //            },
                        //            orderRow2 = new OrderRow()
                        //            {
                        //                countBestBuy = rnd.Next(20, 50),
                        //                priceBestBuy = rnd.Next(3000, 50000),
                        //                volumeBestBuy = rnd.Next(100000, 1000000),
                        //                countBestSale = rnd.Next(20, 50),
                        //                priceBestSale = rnd.Next(3000, 50000),
                        //                volumeBestSale = rnd.Next(100000, 1000000),

                        //            },
                        //            orderRow3 = new OrderRow()
                        //            {
                        //                countBestBuy = rnd.Next(20, 50),
                        //                priceBestBuy = rnd.Next(3000, 50000),
                        //                volumeBestBuy = rnd.Next(100000, 1000000),
                        //                countBestSale = rnd.Next(20, 50),
                        //                priceBestSale = rnd.Next(3000, 50000),
                        //                volumeBestSale = rnd.Next(100000, 1000000),

                        //            },
                        //            orderRow4 = new OrderRow()
                        //            {
                        //                countBestBuy = rnd.Next(20, 50),
                        //                priceBestBuy = rnd.Next(3000, 50000),
                        //                volumeBestBuy = rnd.Next(100000, 1000000),
                        //                countBestSale = rnd.Next(20, 50),
                        //                priceBestSale = rnd.Next(3000, 50000),
                        //                volumeBestSale = rnd.Next(100000, 1000000),

                        //            },
                        //            orderRow5 = new OrderRow()
                        //            {
                        //                countBestBuy = rnd.Next(20, 50),
                        //                priceBestBuy = rnd.Next(3000, 50000),
                        //                volumeBestBuy = rnd.Next(100000, 1000000),
                        //                countBestSale = rnd.Next(20, 50),
                        //                priceBestSale = rnd.Next(3000, 50000),
                        //                volumeBestSale = rnd.Next(100000, 1000000),

                        //            },
                        //            orderRow6 = new OrderRow()
                        //            {
                        //                countBestBuy = rnd.Next(20, 50),
                        //                priceBestBuy = rnd.Next(3000, 50000),
                        //                volumeBestBuy = rnd.Next(100000, 1000000),
                        //                countBestSale = rnd.Next(20, 50),
                        //                priceBestSale = rnd.Next(3000, 50000),
                        //                volumeBestSale = rnd.Next(100000, 1000000),

                        //            }

                        //        };

                        //        var prices = JsonConvert.SerializeObject(model);

                        //        _hubContext.Clients.All.SendCoreAsync("OnChangeTrades", new object[] { prices, c, "ttt" });
                        //    }
                        //    catch (Exception e)
                        //    {

                        //    }
                        //}

                        #endregion

                        var t = consumeResult.Message.Value;
                    }
                    catch (Exception e)
                    {


                    }
                }
                consumer.Close();
            }


        }

        /// <summary>
        ///  topic=> CustomerWallet
        /// </summary>
        /// <returns></returns>
        public async Task PushCustomerWallet()
        {
            using (var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build())
            {
                var c = 0;
                consumer.Subscribe($"CustomerWallet");
                while (true)
                {
                    try
                    {
                        var hubs = _hubConnationService.UserHubsList("CustomerInfoUserId");
                        var consumeResult = consumer.Consume();
                        var model1 = JsonConvert.DeserializeObject<Object>(consumeResult.Message.Value);
                        //var hubs = _hubConnationService.UserHubsList(model1.Customer);

                        if (hubs != null)
                            _hubContext.Clients.Clients(hubs.Hubs).SendCoreAsync("OnUpdateCustomerWallet", new object[] { model1 });

                        _hubContext.Clients.All.SendCoreAsync("OnUpdateCustomerWallet", new object[] { consumeResult.Message.Value });



                        var t = consumeResult.Message.Value;
                    }
                    catch (Exception e)
                    {


                    }
                }
                consumer.Close();

            }
        }

        /// <summary>
        ///  topic=> CustomerPortfolioL
        /// </summary>
        /// <returns></returns>
        public async Task PushCustomerPortfolio()
        {
            using (var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build())
            {
                var c = 0;
                consumer.Subscribe($"CustomerPortfolioL");
                while (true)
                {
                    try
                    {
                        var hubs = _hubConnationService.UserHubsList("CustomerInfoUserId");
                        var consumeResult = consumer.Consume();
                        var model1 = JsonConvert.DeserializeObject<Object>(consumeResult.Message.Value);
                        //var hubs = _hubConnationService.UserHubsList(model1.Customer);

                        if (hubs != null)
                            _hubContext.Clients.Clients(hubs.Hubs).SendCoreAsync("OnUpdateCustomerPortfolio", new object[] { model1 });

                        _hubContext.Clients.All.SendCoreAsync("OnUpdateCustomerPortfolio", new object[] { consumeResult.Message.Value });



                        var t = consumeResult.Message.Value;
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
            if (ConsumerIsStar)
                return;

            Task.Run(() => PushTradeState());
            Task.Run(() => PushOrderAdded());
            Task.Run(() => PushCustomerPortfolio());
            Task.Run(() => PushCustomerWallet());


            ConsumerIsStar = true;
        }

       
    }

}