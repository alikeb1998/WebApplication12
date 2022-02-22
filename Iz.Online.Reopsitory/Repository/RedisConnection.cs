using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;


namespace Iz.Online.Reopsitory.Repository
{
    public static class RedisConnection
    {

        static RedisConnection()
        {
            const string title = "RedisConnection";
           // var urlName = ConfigurationManager.ConnectionStrings[title];
            var urlName = "locahost:6379";
            if (urlName == null)
            {
                throw new Exception("error");
            }
            else
            {
                RedisConnection.connection = new Lazy<ConnectionMultiplexer>(() =>
                {
                    return ConnectionMultiplexer.Connect(urlName.ToString());
                    // return ConnectionMultiplexer.Connect("localhost");
                });
            }
        }


        // static RedisConnection()
        // {
        //      var host = ConfigurationManager.AppSettings["crumus.OTP:Redis:Host"].ToString();
        //
        //      RedisConnection.connection = new Lazy<ConnectionMultiplexer>(() =>
        //      {
        //           return ConnectionMultiplexer.Connect(host);
        //      });
        //      // var url = ConfigurationManager.AppSettings["crumus.OTP:Redis:Host"].ToString();
        //      // var portNumber =  ConfigurationManager.AppSettings["crumus.OTP:Redis:Port"].ToString();
        //      //
        //      // ConfigurationOptions co = new ConfigurationOptions()
        //      // {
        //      // 	SyncTimeout = 500000,
        //      // 	EndPoints =
        //      // 	{
        //      // 		url,
        //      // 		portNumber
        //      // 	},
        //      // 	AbortOnConnectFail = false 
        //      // };
        //
        //      // RedisConnection.connection = new Lazy<ConnectionMultiplexer>(() =>
        //      // {
        //      //
        //      // 	return  ConnectionMultiplexer.Connect(co);
        //      // });
        // }


        private static Lazy<ConnectionMultiplexer> connection;

        //
        public static ConnectionMultiplexer Connection
        {
            get { return connection.Value; }
        }
    }
}