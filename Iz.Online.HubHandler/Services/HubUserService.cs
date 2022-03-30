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
                                 await _hubContext.Clients.All.SendCoreAsync("OnOrderAdded", new object[] { model1 });


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
                              await  _hubContext.Clients.Clients(customerData.Hubs).SendCoreAsync("OnUpdateCustomerWallet", new object[] {JsonConvert.SerializeObject(model1) });

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
            Task.Run(() => PushCustomerWallet_Original());


            ConsumerIsStar = true;
        }


    }

}