namespace Iz.Online.API.Infrastructure
{
    public class CacheData
    {
        public CacheData(WebApplicationBuilder builder)
        {
                var config = builder.Configuration.GetSection("RestApiConfigs");
                RestApiConfigs.Address = config.GetValue<string>("Address");
                RestApiConfigs.Authorization = config.GetValue<string>("Authorization");
        }

        public static class RestApiConfigs
        {
            public static string Address { get; set; }
            public static string Authorization { get; set; }
        }

        public static class Security
        {
            public static string UserName{ get; set; }
            public static string Password { get; set; }

        }
    }
}
