using Iz.Online.DataAccess;
using Iz.Online.Reopsitory.IRepository;
using Iz.Online.Reopsitory.Repository;
using Iz.Online.Services.IServices;
using Iz.Online.Services.Services;
using Iz.Online.ExternalServices.Push.IKafkaPushServices;
using Iz.Online.ExternalServices.Push.KafkaPushServices;
using Iz.Online.ExternalServices.Rest.ExternalService;
using Iz.Online.ExternalServices.Rest.IExternalService;
using Iz.Online.ExternalServices.Rest.Infrastructure;
using Iz.Online.HubHandler.Services;
using Iz.Online.HubHandler.IServices;

namespace Iz.Online.API.Infrastructure
{
    public static class InjectionHandler
    {
        public static IServiceCollection InjectServices(this IServiceCollection services)
        {

            services.AddScoped<OnlineBackendDbContext, OnlineBackendDbContext>();
            services.AddControllers();
            services.AddScoped<IOrderServices, OrderServices>();
            services.AddScoped<IExternalOrderService, ExternalOrderService>();
            services.AddScoped<BaseService, BaseService>();
            services.AddScoped<OnlineBackendDbContext, OnlineBackendDbContext>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IExternalTradeService, ExternalTradeService>();
            services.AddScoped<ITradeServices, TradeService>();

            services.AddScoped<IExternalUserService, ExternalUserService>();
            services.AddScoped<IInstrumentsService, InstrumentsService>();
            services.AddScoped<IExternalInstrumentService, ExternalInstrumentService>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBaseRepository, BaseRepository>();
            services.AddScoped<IInstrumentsRepository, InstrumentsRepository>();
            services.AddScoped<IPushService, PushService>();

            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IHubUserService, HubUserService>();


            return services;

        }
    }
}
