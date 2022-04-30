using Confluent.Kafka;
using Iz.Online.HubConnectionHandler.IServices;

using Iz.Online.OmsModels.ResponsModels.BestLimits;

using Iz.Online.Reopsitory.IRepository;

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
using bestLimitView = Izi.Online.ViewModels.Instruments.BestLimit.BestLimits;
using Microsoft.Extensions.Logging;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using Izi.Online.ViewModels.SignalR;
using Iz.Online.OmsModels.ResponsModels.Order;
using Izi.Online.ViewModels.ValidityType;
using Order = Izi.Online.ViewModels.SignalR.Order;
using CashHelper;

namespace Iz.Online.HubHandler
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
        private readonly IHubConnationService _hubConnationService;
        private readonly Microsoft.AspNetCore.SignalR.IHubContext<MainHub> _hubContext;
        private readonly ILogger<HubUserService> _logger;
        private readonly ICacheService _cacheService;
        private string nationalCode;

        private readonly ConsumerConfig _consumerConfig;
        private static bool ConsumerIsStar = false;
        public HubUserService(IHubConnationService hubConnationService, Microsoft.AspNetCore.SignalR.IHubContext<MainHub> hubContext, ILogger<HubUserService> logger, ICacheService cacheService)
        {

            _hubConnationService = hubConnationService;
            _hubContext = hubContext;
            _consumerConfig = new ConsumerConfig
            {

                BootstrapServers = "192.168.72.222:9092",
                GroupId = "foo",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            _logger = logger;
            _cacheService = cacheService;
        }

        #region BESTLIMIT
        

        /// <summary>
        /// {InstrumentId}-bestLimit
        /// </summary>
        public async Task ConsumeRefreshInstrumentBestLimit_Orginal(string InstrumentId, string nationalCode)
        {
            //InstrumentId = "IRO1FOLD0001"; // <= sample فولاد
            if (nationalCode != null)
            {
                this.nationalCode = nationalCode;
            }


            using (var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build())
            {
                consumer.Subscribe($"{InstrumentId}-bestLimit");

                while (true)
                {
                    //Izi.Online.ViewModels.Instruments.BestLimit.BestLimits
                    var consumeResult = consumer.Consume();
                    var bestLimit = new BestLimitResult();


                    bestLimitView result = new bestLimitView();
                    if (consumeResult.Message.Value != null)
                    {
                        bestLimit = JsonConvert.DeserializeObject<BestLimitResult>(consumeResult.Message.Value);
                        result = new bestLimitView()
                        {
                            changeRow1 = bestLimit.changeRow1,
                            changeRow2 = bestLimit.changeRow2,
                            changeRow3 = bestLimit.changeRow3,
                            changeRow4 = bestLimit.changeRow4,
                            changeRow5 = bestLimit.changeRow5,
                            changeRow6 = bestLimit.changeRow6,
                            orderRow1 = new Izi.Online.ViewModels.Instruments.BestLimit.OrderRow()
                            {
                                countBestBuy = bestLimit.orderRow1 == null ? 0 : bestLimit.orderRow1.countBestBuy,
                                countBestSale = bestLimit.orderRow1 == null ? 0 : bestLimit.orderRow1.countBestSale,
                                priceBestBuy = bestLimit.orderRow1 == null ? 0 : bestLimit.orderRow1.priceBestBuy,
                                priceBestSale = bestLimit.orderRow1 == null ? 0 : bestLimit.orderRow1.priceBestSale,
                                volumeBestBuy = bestLimit.orderRow1 == null ? 0 : bestLimit.orderRow1.volumeBestBuy,
                                volumeBestSale = bestLimit.orderRow1 == null ? 0 : bestLimit.orderRow1.volumeBestSale
                            },
                            orderRow2 = new Izi.Online.ViewModels.Instruments.BestLimit.OrderRow()
                            {
                                countBestBuy = bestLimit.orderRow2 == null ? 0 : bestLimit.orderRow2.countBestBuy,
                                countBestSale = bestLimit.orderRow2 == null ? 0 : bestLimit.orderRow2.countBestSale,
                                priceBestBuy = bestLimit.orderRow2 == null ? 0 : bestLimit.orderRow2.priceBestBuy,
                                priceBestSale = bestLimit.orderRow2 == null ? 0 : bestLimit.orderRow2.priceBestSale,
                                volumeBestBuy = bestLimit.orderRow2 == null ? 0 : bestLimit.orderRow2.volumeBestBuy,
                                volumeBestSale = bestLimit.orderRow2 == null ? 0 : bestLimit.orderRow2.volumeBestSale
                            },
                            orderRow3 = new Izi.Online.ViewModels.Instruments.BestLimit.OrderRow()
                            {
                                countBestBuy = bestLimit.orderRow3 == null ? 0 : bestLimit.orderRow3.countBestBuy,
                                countBestSale = bestLimit.orderRow3 == null ? 0 : bestLimit.orderRow3.countBestSale,
                                priceBestBuy = bestLimit.orderRow3 == null ? 0 : bestLimit.orderRow3.priceBestBuy,
                                priceBestSale = bestLimit.orderRow3 == null ? 0 : bestLimit.orderRow3.priceBestSale,
                                volumeBestBuy = bestLimit.orderRow3 == null ? 0 : bestLimit.orderRow3.volumeBestBuy,
                                volumeBestSale = bestLimit.orderRow3 == null ? 0 : bestLimit.orderRow3.volumeBestSale
                            },
                            orderRow4 = new Izi.Online.ViewModels.Instruments.BestLimit.OrderRow()
                            {
                                countBestBuy = bestLimit.orderRow4 == null ? 0 : bestLimit.orderRow4.countBestBuy,
                                countBestSale = bestLimit.orderRow4 == null ? 0 : bestLimit.orderRow4.countBestSale,
                                priceBestBuy = bestLimit.orderRow4 == null ? 0 : bestLimit.orderRow4.priceBestBuy,
                                priceBestSale = bestLimit.orderRow4 == null ? 0 : bestLimit.orderRow4.priceBestSale,
                                volumeBestBuy = bestLimit.orderRow4 == null ? 0 : bestLimit.orderRow4.volumeBestBuy,
                                volumeBestSale = bestLimit.orderRow4 == null ? 0 : bestLimit.orderRow4.volumeBestSale
                            },
                            orderRow5 = new Izi.Online.ViewModels.Instruments.BestLimit.OrderRow()
                            {
                                countBestBuy = bestLimit.orderRow5 == null ? 0 : bestLimit.orderRow5.countBestBuy,
                                countBestSale = bestLimit.orderRow5 == null ? 0 : bestLimit.orderRow5.countBestSale,
                                priceBestBuy = bestLimit.orderRow5 == null ? 0 : bestLimit.orderRow5.priceBestBuy,
                                priceBestSale = bestLimit.orderRow5 == null ? 0 : bestLimit.orderRow5.priceBestSale,
                                volumeBestBuy = bestLimit.orderRow5 == null ? 0 : bestLimit.orderRow5.volumeBestBuy,
                                volumeBestSale = bestLimit.orderRow5 == null ? 0 : bestLimit.orderRow5.volumeBestSale
                            },
                            orderRow6 = new Izi.Online.ViewModels.Instruments.BestLimit.OrderRow()
                            {
                                countBestBuy = bestLimit.orderRow6 == null ? 0 : bestLimit.orderRow6.countBestBuy,
                                countBestSale = bestLimit.orderRow6 == null ? 0 : bestLimit.orderRow6.countBestSale,
                                priceBestBuy = bestLimit.orderRow6 == null ? 0 : bestLimit.orderRow6.priceBestBuy,
                                priceBestSale = bestLimit.orderRow6 == null ? 0 : bestLimit.orderRow6.priceBestSale,
                                volumeBestBuy = bestLimit.orderRow6 == null ? 0 : bestLimit.orderRow6.volumeBestBuy,
                                volumeBestSale = bestLimit.orderRow6 == null ? 0 : bestLimit.orderRow6.volumeBestSale
                            },
                        };
                        result = Izi.Online.ViewModels.Instruments.BestLimit.VolumeProccessor.ProccessVolume(result);

                    }

                    try
                    {
                      
                        await _hubContext.Clients.Group($"instruments/{InstrumentId}").SendAsync("OnRefreshInstrumentBestLimit", result);
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                    //}
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

        public async Task PushPrice_Original(string InstrumentId, string nationalCode)
        {
            if (nationalCode != null)
            {
                this.nationalCode = nationalCode;
            }
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

                        //var hubs = await _hubConnationService.GetInstrumentHubs(InstrumentId);
                        //if (hubs != null)
                        var lastModel = JsonConvert.SerializeObject(res);
                        await _hubContext.Clients.Group($"{InstrumentId}-price").SendAsync($"{InstrumentId}-price", lastModel);

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

        //public async Task PushOrderAdded_Original(string nationalCode)
        public async Task PushTradeState_Original(string nationalCode)
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
                        //var res = new Order()
                        //{

                        //    state = model1.TradedState switch { 1 => "لغو شده", 2 => "سفارش به طور کامل اجرا شده است", 3 => "خطای هسته معاملات", 4 => "منقضی شده", 5 => "انجام شده", 6 => "در حال انتظار", 7 => "در صف", 8 => "در صف در انتظار تایید لغو", 9 => "در صف در انتظار تایید ویرایش", 10 => "قسمتی انجام شده", 11 => "رد شده", },
                        //    instrumentId = _cacheService.InstrumentData(model1.Instrument).Id,
                        //    createdAt = model1.TradedAt,
                        //    executedQ = model1.ExecutedQuantity,
                        //    price = model1.TradedPrice,
                        //    orderSide =model1.OrderSide,
                        //    orderSideText = model1.OrderSide == 2 ? "خرید" : "فروش",
                        //    orderId = model1.OrderId,

                        //};
                        var res = new Izi.Online.ViewModels.SignalR.Trade()
                        {
                            stateText = model1.TradedState switch { 1 => "لغو شده", 2 => "سفارش به طور کامل اجرا شده است", 3 => "خطای هسته معاملات", 4 => "منقضی شده", 5 => "انجام شده", 6 => "در حال انتظار", 7 => "در صف", 8 => "در صف در انتظار تایید لغو", 9 => "در صف در انتظار تایید ویرایش", 10 => "قسمتی انجام شده", 11 => "رد شده", },
                            state = model1.TradedState,
                            instrumentId = _cacheService.InstrumentData(model1.Instrument).Id,
                            tradedAt = model1.TradedAt,
                            executedQ = model1.ExecutedQuantity,
                            price = model1.TradedPrice,
                            orderSide = model1.OrderSide,
                            orderSideText = model1.OrderSide == 2 ? "خرید" : "فروش",
                            tradeId = model1.OrderId,
                            name="tst",
                            
                        };

                        //await _hubContext.Clients.Group(nationalCode).SendCoreAsync("OnOrderAdded", new object[] { res });
                        await _hubContext.Clients.Group(nationalCode).SendAsync("OnChangeTrades", JsonConvert.SerializeObject(res));
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

        // public async Task PushTradeState_Original(string nationalCode)
        public async Task PushOrderAdded_Original(string nationalCode)
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
                        var res = new Order()
                        {
                            state = model1.State switch { "1" => "لغو شده", "2" => "سفارش به طور کامل اجرا شده است", "3" => "خطای هسته معاملات", "4" => "منقضی شده", "5" => "انجام شده", "6" => "در حال انتظار", "7" => "در صف", "8" => "در صف در انتظار تایید لغو", "9" => "در صف در انتظار تایید ویرایش", "10" => "قسمتی انجام شده", "11" => "رد شده", },
                            orderSide = Convert.ToInt32(model1.OrderSide),
                            validityType = Convert.ToInt32(model1.ValidityType),
                            validityInfo = new ValidityInfo() { ValidityDate = model1.ValidityInfo },
                            remainedQ = (long)Convert.ToDouble(model1.RemainingQuantity),
                            quantity = (long)Convert.ToDouble(model1.Quantity),
                            orderId = Convert.ToInt32(model1.OrderId),
                            instrumentId = _cacheService.InstrumentData(model1.Instrument).Id,
                            createdAt = Convert.ToDateTime(model1.ChangedAt),
                            executedQ = Convert.ToInt32(model1.ExecutedQuantity),
                            price = Convert.ToInt32(model1.Price),
                           // stateText = model1.State switch { "1" => "لغو شده", "2" => "سفارش به طور کامل اجرا شده است", "3" => "خطای هسته معاملات", "4" => "منقضی شده", "5" => "انجام شده", "6" => "در حال انتظار", "7" => "در صف", "8" => "در صف در انتظار تایید لغو", "9" => "در صف در انتظار تایید ویرایش", "10" => "قسمتی انجام شده", "11" => "رد شده", },
                            instrumentName = "test",
                            orderSideText = model1.OrderSide.Equals("1") ? "فروش" : "خرید",
                            
                        };
                        res.executePercent = res.executedQ / res.quantity * 100;
                        //await _hubContext.Clients.Group(model1.Customer).SendCoreAsync("OnChangeTrades", new object[] { res });
                        await _hubContext.Clients.Group(model1.Customer).SendAsync("OnOrderAdded", JsonConvert.SerializeObject(res));

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

        public async Task PushCustomerWallet_Original(string nationalCode)
        {
            using (var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build())
            {

                consumer.Subscribe($"CustomerWallet");
                while (true)
                {

                    try
                    {


                        var consumeResult = consumer.Consume();
                      
                        var model1 = JsonConvert.DeserializeObject<CustomerWallet>(consumeResult.Message.Value);
                        var res =new Wallet()
                        {
                            blockedValue =model1.BlockedValue,
                            buyingPower = model1.BuyingPower,
                            withdrawable =model1.Withdrawable,
                            lendedCredit =model1.LendedCredit,
                            nonWithdrawable = model1.NonWithdrawable,
                        };
                        var LastModel = JsonConvert.SerializeObject(res);
                        await _hubContext.Clients.Group(model1.Customer).SendAsync("OnUpdateCustomerWallet", LastModel  );

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

        #region PORTFOLIO
        /// <summary>
        ///  topic=> CustomerPortfolioL
        /// </summary>
        /// <returns></returns>

        public async Task PushCustomerPortfolio_Original(string nationalCode)
        {
            using (var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build())
            {

                consumer.Subscribe("CustomerPortfolio");

                while (true)
                {
                    try
                    {
                        var consumeResult = consumer.Consume();
                        var model1 = JsonConvert.DeserializeObject<Portfolio>(consumeResult.Message.Value);
                        var res = new Asset()
                        {
                            Name = "test",
                            LastPrice = model1.LastPrice,
                            TradeableQuantity = model1.Quantity,
                            Gav = 0,
                            AvgPrice = model1.AveragePrice,
                            FianlAmount = model1.HeadToHeadPoint,
                            ProfitAmount = model1.profitLoss,
                            ProfitPercent = model1.ProfitLossPercent,
                            SellProfit = 0,
                            InstrumentId = _cacheService.InstrumentData(model1.InstrumentCode).Id,

                        };
                        var lastModel = JsonConvert.SerializeObject(res).ToLower();
                            await _hubContext.Clients.Group(model1.NationalId).SendAsync("OnUpdateCustomerPortfolio", lastModel);

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



        public async Task CreateAllConsumers(string nationalCode)
        {
            //if (ConsumerIsStar)
            //    return;
            List<Task> tasks = new();
            tasks.Add(Task.Run(async () => await PushTradeState_Original(nationalCode)));
            tasks.Add(Task.Run(async () => await PushOrderAdded_Original(nationalCode)));
            tasks.Add(Task.Run(async () => await PushCustomerPortfolio_Original(this.nationalCode)));
            tasks.Add(Task.Run(async () => await PushCustomerWallet_Original(nationalCode)));
            Task.WhenAll(tasks);


            //ConsumerIsStar = true;
        }

        public static DateTime GetTime(string dateTime)
        {
            if (string.IsNullOrEmpty(dateTime))
                return DateTime.Now;

            var year = Convert.ToInt32(dateTime.Substring(0, 4));
            var month = Convert.ToInt32(dateTime.Substring(4, 2));
            var day = Convert.ToInt32(dateTime.Substring(6, 2));
            var hour = Convert.ToInt32(dateTime.Substring(8, 2));
            var minute = Convert.ToInt32(dateTime.Substring(10, 2));
            var sec = Convert.ToInt32(dateTime.Substring(12, 2));

            DateTime res = new DateTime(year, month, day, hour, minute, sec);


            return res;
        }
    }

}