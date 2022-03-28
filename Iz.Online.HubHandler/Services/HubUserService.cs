using Confluent.Kafka;
using Iz.Online.HubConnectionHandler.IServices;
using Iz.Online.HubHandler.IServices;
using Iz.Online.OmsModels.ResponsModels.BestLimits;

using Iz.Online.Reopsitory.IRepository;
using Iz.Online.SignalR;
using Izi.Online.ViewModels.Instruments;
using Izi.Online.ViewModels.Orders;
using Izi.Online.ViewModels.Trades;
using Izi.Online.ViewModels.Users;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using OrderChanged = Iz.Online.OmsModels.ResponsModels.Order.OrderChangeTopic;
using Detail = Iz.Online.OmsModels.ResponsModels.Kafka.InstrumentDetail;
using PushDetail = Izi.Online.ViewModels.SignalR.InstrumentDetail;
using BestLimitResult = Izi.Online.ViewModels.SignalR.BestLimit;
using Iz.Online.OmsModels.ResponsModels.Kafka;

namespace Iz.Online.HubHandler.Services
{

    //topics =>
    //OrderChange (active order tab ) ,\
    //OrderTrade  (today trades tab) , \
    //{InstrumentId}-bestLimit ,       \
    //CustomerWallet ,                 \
    //CustomerPortfolioL  ,            \
    //{InstrumentId}-price (details)   \




    public class HubUserService : IHubUserService
    {
        public static bool isFakeData = false;
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

        #region BESTLIMIT
        public async Task ConsumeRefreshInstrumentBestLimit(string InstrumentId)
        {

            var c = 0;
            while (c < 50)
            {
                //Task.Run(async () => CreateAllConsumers());
                c++;
                try
                {
                    Thread.Sleep(10000);
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
                            BuyWeight = 50,
                            SellWeight = 0

                        },
                        orderRow2 = new Izi.Online.ViewModels.Instruments.BestLimit.OrderRow()
                        {
                            countBestBuy = rnd.Next(20, 50),
                            priceBestBuy = rnd.Next(3000, 50000),
                            volumeBestBuy = rnd.Next(100000, 1000000),
                            countBestSale = rnd.Next(20, 50),
                            priceBestSale = rnd.Next(3000, 50000),
                            volumeBestSale = rnd.Next(100000, 1000000),
                            SellWeight = 20,
                            BuyWeight = 0
                        },
                        orderRow3 = new Izi.Online.ViewModels.Instruments.BestLimit.OrderRow()
                        {
                            countBestBuy = rnd.Next(20, 50),
                            priceBestBuy = rnd.Next(3000, 50000),
                            volumeBestBuy = rnd.Next(100000, 1000000),
                            countBestSale = rnd.Next(20, 50),
                            priceBestSale = rnd.Next(3000, 50000),
                            volumeBestSale = rnd.Next(100000, 1000000),
                            BuyWeight = 50,
                            SellWeight = 0

                        },
                        orderRow4 = new Izi.Online.ViewModels.Instruments.BestLimit.OrderRow()
                        {
                            countBestBuy = rnd.Next(20, 50),
                            priceBestBuy = rnd.Next(3000, 50000),
                            volumeBestBuy = rnd.Next(100000, 1000000),
                            countBestSale = rnd.Next(20, 50),
                            priceBestSale = rnd.Next(3000, 50000),
                            volumeBestSale = rnd.Next(100000, 1000000),
                            SellWeight = 20,
                            BuyWeight = 0,
                        },
                        orderRow5 = new Izi.Online.ViewModels.Instruments.BestLimit.OrderRow()
                        {
                            countBestBuy = rnd.Next(20, 50),
                            priceBestBuy = rnd.Next(3000, 50000),
                            volumeBestBuy = rnd.Next(100000, 1000000),
                            countBestSale = rnd.Next(20, 50),
                            priceBestSale = rnd.Next(3000, 50000),
                            volumeBestSale = rnd.Next(100000, 1000000),
                            BuyWeight = 50,
                            SellWeight = 0,


                        },
                        orderRow6 = new Izi.Online.ViewModels.Instruments.BestLimit.OrderRow()
                        {
                            countBestBuy = rnd.Next(20, 50),
                            priceBestBuy = rnd.Next(3000, 50000),
                            volumeBestBuy = rnd.Next(100000, 1000000),
                            countBestSale = rnd.Next(20, 50),
                            priceBestSale = rnd.Next(3000, 50000),
                            volumeBestSale = rnd.Next(100000, 1000000),
                            SellWeight = 20,
                            BuyWeight = 0,
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
                    var prices = new BestLimitResult();
                    if (consumeResult.Message.Value != null)
                    {
                        prices = JsonConvert.DeserializeObject<BestLimitResult>(consumeResult.Message.Value);


                    }

                    //TODO cast to Izi.Online.ViewModels.Instruments.BestLimit.BestLimits

                    var hubs = _hubConnationService.GetInstrumentHubs(InstrumentId);
                    if (hubs != null)
                        await _hubContext.Clients.Clients(hubs).SendCoreAsync("OnRefreshInstrumentBestLimit", new object[] { prices, InstrumentId, " " });
                }
                consumer.Close();
            }

        }
        #endregion

        #region PRICE
        /// <summary>
        ///  topic=> {InstrumentId}-price
        /// </summary>
        /// <returns></returns>
        public async Task PushPrice(string InstrumentId)
        {
            //using (var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build())
            //{

            //    var c = 0;
            //    consumer.Subscribe($"{InstrumentId}-price");
            //    consumer.Subscribe($"{InstrumentId}-detailed");
            //    while (true)
            //    {
            //        try
            //        {
            //            Thread.Sleep(5000);
            //            var rnd = new Random();
            //            //var hubs = _hubConnationService.UserHubsList("CustomerInfoUserId");
            //            //var consumeResult = consumer.Consume();
            //            //var model1 = JsonConvert.DeserializeObject<object>(consumeResult.Message.Value);
            //            //var hubs = _hubConnationService.UserHubsList(model1.Customer);

            //            //if (hubs != null)
            //            //    _hubContext.Clients.Clients(hubs.Hubs).SendCoreAsync($"{model1}-price", new object[] { model1, $"InstrumentId : '{model1}{c}'", " " });

            //            //_hubContext.Clients.All.SendCoreAsync($"{model1}-price", new object[] { consumeResult.Message.Value });

            //            var model = new InstrumentDetail()
            //            {


            //                closingPrice = rnd.NextDouble(),//
            //                firstPrice = rnd.NextDouble(),//
            //                GroupState = 1,//

            //                highPrice = rnd.NextDouble(),
            //                lastPrice = rnd.NextDouble(),//

            //                lastTradeDate = DateTime.Today,//
            //                lowPrice = rnd.NextDouble(),

            //                numberOfTrades = 1,//
            //                PriceMax = rnd.NextInt64(100, 1000),//
            //                PriceMin = rnd.NextInt64(100, 1000),//

            //                State = 1,//


            //                valueOfTrades = 1,//
            //                volumeOfTrades = 1,//
            //                yesterdayPrice = rnd.NextDouble(),//

            //            };
            //            var res = JsonConvert.SerializeObject(model);
            //            _hubContext.Clients.All.SendCoreAsync($"{InstrumentId}-price", new object[] { res });
            //            //var t = consumeResult.Message.Value;
            //            var t = "test";
            //        }
            //        catch (Exception e)
            //        {


            //        }
            //    }
            //    consumer.Close();
            //}



        }

        public async Task PushPrice_Original(string InstrumentId)
        {
            using (var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build())
            {
                consumer.Subscribe($"{InstrumentId}-price");
                while (true)
                {
                    try
                    {

                        var consumeResult = consumer.Consume();
                        var model = new Detail();
                        var res = new PushDetail();
                        if (consumeResult.Message.Value != null)
                        {
                            model = JsonConvert.DeserializeObject<Detail>(consumeResult.Message.Value);
                            res.yesterdayPrice = model.yesterdayPrice;
                            res.volumeOfTrades = model.volumeOfTrades;
                            res.valueOfTrades = model.valueOfTrades;
                            res.numberOfTrades = model.numberOfTrades;
                            res.highPrice = model.maximumPrice;
                            res.lowPrice = model.minimumPrice;
                            res.closingPrice = model.closingPrice;
                            res.lastTradeDate = model.lastTradeDate;
                            res.lastPrice = model.lastPrice;
                            res.firstPrice = model.firstPrice;
                            ///
                            



                        }

                        var hubs = _hubConnationService.GetInstrumentHubs(InstrumentId);
                        if (hubs != null)
                            await _hubContext.Clients.Clients(hubs).SendCoreAsync($"{InstrumentId}-price", new object[] { res, InstrumentId, " " });



                        //var t = consumeResult.Message.Value;

                    }
                    catch (Exception e)
                    {


                    }
                }
                consumer.Close();
            }



        }

        #endregion

        #region ADDORDER
        /// <summary>
        /// topic => OrderTrade
        /// </summary>
        public async Task PushOrderAdded()
        {
            using (var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build())
            {

                //consumer.Subscribe($"OrderTrade");
                while (true)
                {
                    try
                    {
                        Thread.Sleep(10000);
                        var rnd = new Random();
                        //var consumeResult = consumer.Consume();
                        //var t = consumeResult.Message.Value;
                        // _hubContext.Clients.All.SendCoreAsync("OnOrderAdded", new object[] { consumeResult.Message.Value });

                        var active = new ActiveOrder()
                        {
                            CreatedAt = DateTime.Now,
                            ExecutedQ = rnd.NextInt64(1, 100),
                            ExecutePercent = rnd.NextDouble(),
                            InstrumentId = 12,
                            InstrumentName = "foolaad",
                            NscCode = "IRO1fold0001",
                            OrderQtyWait = rnd.NextInt64(1, 100),
                            OrderSide = 1,
                            OrderSideText = "sell",
                            Price = rnd.NextInt64(1, 100),
                            Quantity = rnd.NextInt64(1, 100),
                            RemainedQ = rnd.NextInt64(1, 100),
                            State = "در صف",
                            StateText = "fdf",
                            ValidityType = 1,

                        };
                        var res = JsonConvert.SerializeObject(active);
                        _hubContext.Clients.All.SendCoreAsync("OnOrderAdded", new object[] { res });

                    }
                    catch (Exception e)
                    {

                    }
                }
                consumer.Close();
            }

        }

        public  Task PushOrderAdded_Original()
        {
            using (var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build())
            {

                consumer.Subscribe($"OrderTrade");
                while (true)
                {
                    try
                    {
                        var consumeResult = consumer.Consume();
                        var model1 = JsonConvert.DeserializeObject<AddOrder>(consumeResult.Message.Value);

                        var customerData = _hubConnationService.UserHubsList(model1.Customer);

                        if (customerData != null)
                            if (customerData.Hubs.Count > 0)
                                 _hubContext.Clients.All.SendCoreAsync("OnOrderAdded", new object[] { model1 });




                        var t = consumeResult.Message.Value;

                    }
                    catch (Exception e)
                    {

                    }
                }
                consumer.Close();
            }

        }

        #endregion

        #region ORDERSTATE
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
                        //    var hubs = _hubConnationService.UserHubsList("CustomerInfoUserId");
                        //    var consumeResult = consumer.Consume();
                        //var model1 = JsonConvert.DeserializeObject<OrderChanged>(consumeResult.Message.Value);
                        //var hubs = _hubConnationService.UserHubsList(model1.Customer);

                        //if (hubs != null)
                        //    _hubContext.Clients.Clients(hubs.Hubs).SendCoreAsync("OnChangeTrades", new object[] { model1.Price, $"InstrumentId : '{model1.Instrument}{c}'", " " });

                        //_hubContext.Clients.All.SendCoreAsync("OnChangeTrades", new object[] { consumeResult.Message.Value });

                        #region fake data

                        while (true)
                        {
                            c++;
                            try
                            {
                                Random rnd = new Random();
                                Thread.Sleep(10000);
                                #region f
                                //OmsModels.ResponsModels.BestLimits.BestLimit model = new OmsModels.ResponsModels.BestLimits.BestLimit()
                                //{
                                //    changeRow1 = rnd.Next(20, 30) > 25,
                                //    changeRow2 = rnd.Next(20, 30) > 25,
                                //    changeRow3 = rnd.Next(20, 30) > 25,
                                //    changeRow4 = rnd.Next(20, 30) > 25,
                                //    changeRow5 = rnd.Next(20, 30) > 25,
                                //    changeRow6 = rnd.Next(20, 30) > 25,
                                //    orderRow1 = new OrderRow()
                                //    {
                                //        countBestBuy = rnd.Next(20, 50),
                                //        priceBestBuy = rnd.Next(3000, 50000),
                                //        volumeBestBuy = rnd.Next(100000, 1000000),
                                //        countBestSale = rnd.Next(20, 50),
                                //        priceBestSale = rnd.Next(3000, 50000),
                                //        volumeBestSale = rnd.Next(100000, 1000000),

                                //    },
                                //    orderRow2 = new OrderRow()
                                //    {
                                //        countBestBuy = rnd.Next(20, 50),
                                //        priceBestBuy = rnd.Next(3000, 50000),
                                //        volumeBestBuy = rnd.Next(100000, 1000000),
                                //        countBestSale = rnd.Next(20, 50),
                                //        priceBestSale = rnd.Next(3000, 50000),
                                //        volumeBestSale = rnd.Next(100000, 1000000),

                                //    },
                                //    orderRow3 = new OrderRow()
                                //    {
                                //        countBestBuy = rnd.Next(20, 50),
                                //        priceBestBuy = rnd.Next(3000, 50000),
                                //        volumeBestBuy = rnd.Next(100000, 1000000),
                                //        countBestSale = rnd.Next(20, 50),
                                //        priceBestSale = rnd.Next(3000, 50000),
                                //        volumeBestSale = rnd.Next(100000, 1000000),

                                //    },
                                //    orderRow4 = new OrderRow()
                                //    {
                                //        countBestBuy = rnd.Next(20, 50),
                                //        priceBestBuy = rnd.Next(3000, 50000),
                                //        volumeBestBuy = rnd.Next(100000, 1000000),
                                //        countBestSale = rnd.Next(20, 50),
                                //        priceBestSale = rnd.Next(3000, 50000),
                                //        volumeBestSale = rnd.Next(100000, 1000000),

                                //    },
                                //    orderRow5 = new OrderRow()
                                //    {
                                //        countBestBuy = rnd.Next(20, 50),
                                //        priceBestBuy = rnd.Next(3000, 50000),
                                //        volumeBestBuy = rnd.Next(100000, 1000000),
                                //        countBestSale = rnd.Next(20, 50),
                                //        priceBestSale = rnd.Next(3000, 50000),
                                //        volumeBestSale = rnd.Next(100000, 1000000),

                                //    },
                                //    orderRow6 = new OrderRow()
                                //    {
                                //        countBestBuy = rnd.Next(20, 50),
                                //        priceBestBuy = rnd.Next(3000, 50000),
                                //        volumeBestBuy = rnd.Next(100000, 1000000),
                                //        countBestSale = rnd.Next(20, 50),
                                //        priceBestSale = rnd.Next(3000, 50000),
                                //        volumeBestSale = rnd.Next(100000, 1000000),

                                //    }

                                //};
                                #endregion
                                var model = new List<Trade>();
                                var r = new Trade()
                                {
                                    ExecutedQ = rnd.NextInt64(1, 100),
                                    InstrumentId = 121,
                                    Name = "fooolaad",
                                    NscCode = "sdfafdf",
                                    OrderSide = 1,
                                    Price = rnd.NextInt64(1, 1000),
                                    Quantity = rnd.NextDouble(),
                                    State = "texxt",
                                    TradedAt = DateTime.Now,
                                    TradeId = rnd.NextInt64(1, 100),
                                    TradeValue = rnd.NextDouble(),
                                };
                                for (int i = 0; i < 3; i++)
                                {
                                    model.Add(r);
                                }
                                var prices = JsonConvert.SerializeObject(model);

                                _hubContext.Clients.All.SendCoreAsync("OnChangeTrades", new object[] { prices });
                            }
                            catch (Exception e)
                            {

                            }
                        }

                        #endregion

                        // var t = consumeResult.Message.Value;
                        var t = "test";
                    }
                    catch (Exception e)
                    {


                    }
                }
                consumer.Close();
            }


        }

        public async Task PushTradeState_Original()
        {
            using (var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build())
            {
                consumer.Subscribe($"OrderChange");
                while (true)
                {
                    try
                    {
                        var consumeResult = consumer.Consume();
                        var model1 = JsonConvert.DeserializeObject<OrderChange>(consumeResult.Message.Value);

                        var customerData = _hubConnationService.UserHubsList(model1.Customer);

                        if (customerData != null)
                            if (customerData.Hubs.Count > 0)
                           await  _hubContext.Clients.Clients(customerData.Hubs).SendCoreAsync("OnChangeTrades", new object[] { model1 });

                        var t = consumeResult.Message.Value;

                    }
                    catch (Exception e)
                    {


                    }
                }
                consumer.Close();
            }
        }
        #endregion

        #region WALLET
        /// <summary>
        ///  topic=> CustomerWallet
        /// </summary>
        /// <returns></returns>
        public async Task PushCustomerWallet()
        {
            using (var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build())
            {
                var c = 0;
                // consumer.Subscribe($"CustomerWallet");
                while (true)
                {

                    try
                    {

                        Thread.Sleep(10000);
                        Random rnd = new Random();
                        var wallet = new Wallet()
                        {

                            BlockedValue = rnd.NextInt64(100, 1000),
                            BuyingPower = rnd.NextInt64(100, 1000),
                            LendedCredit = rnd.NextInt64(100, 1000),
                            NonWithdrawable = rnd.NextInt64(100, 1000),
                            Withdrawable = rnd.NextInt64(100, 1000)
                        };
                        var prices = JsonConvert.SerializeObject(wallet);
                        //var hubs = _hubConnationService.UserHubsList("CustomerInfoUserId");
                        //  var consumeResult = consumer.Consume();
                        //var model1 = JsonConvert.DeserializeObject<Object>(consumeResult.Message.Value);
                        //var hubs = _hubConnationService.UserHubsList(model1.Customer);
                        //var hubs = _hubConnationService.UserHubsList("KafkaUserId");
                        //if (hubs != null)
                        // _hubContext.Clients.Clients("KafkaUserId").SendCoreAsync("OnUpdateCustomerWallet", new object[] { prices });

                        _hubContext.Clients.All.SendCoreAsync("OnUpdateCustomerWallet", new object[] { prices });



                        var t = "test";
                    }
                    catch (Exception e)
                    {


                    }
                }
                consumer.Close();

            }
        }

        public async Task PushCustomerWallet_Original()
        {
            using (var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build())
            {

                consumer.Subscribe($"CustomerWallet");
                while (true)
                {

                    try
                    {

                        #region comment
                        //var consumeResult = consumer.Consume();
                        //var model1 = JsonConvert.DeserializeObject<Object>(consumeResult.Message.Value);
                        

                        //var customerData = _hubConnationService.UserHubsList(model1.Customer);

                        //if (customerData != null)
                        //    if (customerData.Hubs.Count > 0)
                        //        await _hubContext.Clients.Clients(customerData.Hubs).SendCoreAsync("OnUpdateCustomerWallet", new object[] {JsonConvert.SerializeObject(model1) });

                        //var t = consumeResult.Message.Value; 
                        #endregion
                    }
                    catch (Exception e)
                    {

                    }
                }
                consumer.Close();

            }
        }

        #endregion

        #region PORTFOLIO
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
                        Random rnd = new Random();
                        Thread.Sleep(10000);
                        //var hubs = _hubConnationService.UserHubsList("CustomerInfoUserId");
                        //var consumeResult = consumer.Consume();
                        //var model1 = JsonConvert.DeserializeObject<Object>(consumeResult.Message.Value);
                        //var hubs = _hubConnationService.UserHubsList(model1.Customer);

                        //if (hubs != null)
                        //    _hubContext.Clients.Clients(hubs.Hubs).SendCoreAsync("OnUpdateCustomerPortfolio", new object[] { model1 });

                        //_hubContext.Clients.All.SendCoreAsync("OnUpdateCustomerPortfolio", new object[] { consumeResult.Message.Value });
                        // var portfo = new 
                        var model = new List<Asset>();
                        var pr = new Asset()
                        {
                            AvgPrice = rnd.NextInt64(100, 1000),
                            FianlAmount = rnd.NextInt64(100, 1000),
                            InstrumentId = rnd.Next(100, 1000),
                            LastPrice = rnd.NextInt64(100, 1000),
                            NscCode = "FOLD0001",
                            ProfitAmount = rnd.NextInt64(100, 1000),
                            Name = "fooolad",
                            ProfitPercent = 10,
                            SellProfit = 12,
                            TradeableQuantity = rnd.NextInt64(100, 1000),
                            Gav = rnd.NextInt64(100, 1000),
                        };
                        for (int i = 0; i < 3; i++)
                        {
                            model.Add(pr);
                        }
                        var res = JsonConvert.SerializeObject(model);
                        _hubContext.Clients.All.SendCoreAsync("OnUpdateCustomerPortfolio", new object[] { model });





                        var t = "test";
                    }
                    catch (Exception e)
                    {


                    }
                }
                consumer.Close();

            }
        }

        public async Task PushCustomerPortfolio_Original()
        {
            using (var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build())
            {

                consumer.Subscribe($"CustomerPortfolioL");

                while (true)
                {
                    try
                    {


                        var consumeResult = consumer.Consume();
                        var model1 = JsonConvert.DeserializeObject<Portfolio>(consumeResult.Message.Value);
                        var hubs = _hubConnationService.UserHubsList(model1.NationalId);

                        if (hubs != null)
                           await  _hubContext.Clients.Clients(hubs.Hubs).SendCoreAsync("OnUpdateCustomerPortfolio", new object[] { model1 });

                        var t = consumeResult.Message.Value;
                    }
                    catch (Exception e)
                    {


                    }
                }
                consumer.Close();

            }
        }
        #endregion

        public async Task CreateAllConsumers()
        {
            if (ConsumerIsStar)
                return;

            Task.Run(() => PushTradeState_Original());
            Task.Run(() => PushOrderAdded_Original());
            Task.Run(() => PushCustomerPortfolio_Original());
            Task.Run(() => PushCustomerWallet());


            ConsumerIsStar = true;
        }


    }

}