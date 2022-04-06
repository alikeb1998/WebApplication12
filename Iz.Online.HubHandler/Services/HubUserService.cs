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
using bestLimitView = Izi.Online.ViewModels.Instruments.BestLimit.BestLimits;
using Microsoft.Extensions.Logging;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;

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
        private readonly Microsoft.AspNetCore.SignalR.IHubContext<CustomersHub> _hubContext;
        private readonly ILogger<HubUserService> _logger;
        
      
        private readonly ConsumerConfig _consumerConfig;
        private static bool ConsumerIsStar = false;
        public HubUserService(IHubConnationService hubConnationService, Microsoft.AspNetCore.SignalR.IHubContext<CustomersHub> hubContext, ILogger<HubUserService> logger)
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

        }

        #region BESTLIMIT


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
                    var bestLimit = new BestLimitResult();
                    _logger.LogError("bestLimit init :" + DateTime.Now.ToString());

                    bestLimitView result = new bestLimitView();
                    if (consumeResult.Message.Value != null)
                    {
                        bestLimit = JsonConvert.DeserializeObject<BestLimitResult>(consumeResult.Message.Value);
                       result =new bestLimitView()
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

                    var hubs = await _hubConnationService.GetInstrumentHubs(InstrumentId);
                    if (hubs != null)
                    {
                        try
                        {
                            await _hubContext.Clients.Clients(hubs).SendCoreAsync("OnRefreshInstrumentBestLimit", new object[] { result, InstrumentId, " " });

                        }
                        catch (Exception ex)
                        {

                            throw;
                        }
                    }
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

                        var hubs = await _hubConnationService.GetInstrumentHubs(InstrumentId);
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

        public async Task PushOrderAdded_Original()
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
                                await _hubContext.Clients.Clients(customerData.Hubs).SendCoreAsync("OnOrderAdded", new object[] { model1 });


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
                                await _hubContext.Clients.Clients(customerData.Hubs).SendCoreAsync("OnChangeTrades", new object[] { model1 });

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
                        var consumeResult = consumer.Consume();
                        var model1 = JsonConvert.DeserializeObject<CustomerWallet>(consumeResult.Message.Value);


                        var customerData = _hubConnationService.UserHubsList(model1.Customer);

                        if (customerData != null)
                            if (customerData.Hubs.Count > 0)
                                await _hubContext.Clients.Clients(customerData.Hubs).SendCoreAsync("OnUpdateCustomerWallet", new object[] { JsonConvert.SerializeObject(model1) });

                        var t = consumeResult.Message.Value;
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
                            await _hubContext.Clients.Clients(hubs.Hubs).SendCoreAsync("OnUpdateCustomerPortfolio", new object[] { model1 });

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
            Task.Run(() => PushCustomerWallet_Original());


            ConsumerIsStar = true;
        }


    }

}