using System.Configuration;
using Iz.Online.API.Controllers;
using Iz.Online.API.Infrastructure;
using Iz.Online.Reopsitory.IRepository;
using Iz.Online.Reopsitory.Repository;
using Iz.Online.Services.IServices;
using Iz.Online.Services.Services;
using Iz.Online.DataAccess;
using Iz.Online.ExternalServices.Push.IKafkaPushServices;
using Iz.Online.ExternalServices.Push.KafkaPushServices;
using Iz.Online.ExternalServices.Rest.ExternalService;
using Iz.Online.ExternalServices.Rest.IExternalService;
using Iz.Online.ExternalServices.Rest.Infrastructure;
using Iz.Online.Services.Infrastructure;
using Iz.Online.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

var corsOrigins = new List<string>()
{
    "http://192.168.72.112:4444/" , "http://localhost:4444/" , "http://127.0.0.1:4444/",
    "http://192.168.72.112:5555/" , "http://localhost:5555/" , "http://127.0.0.1:5555/",

    "http://192.168.72.112:4444" , "http://localhost:4444" , "http://127.0.0.1:4444",
    "http://192.168.72.112:5555" , "http://localhost:5555" , "http://127.0.0.1:5555",

    "http://localhost:3000" , "http://localhost:3000"
};

builder.Services.AddCors(options =>
{
    options.AddPolicy("CustomCors", policy =>
    {
        policy.SetIsOriginAllowedToAllowWildcardSubdomains();

        foreach (string item in corsOrigins)
        {
            policy.WithOrigins(item).AllowAnyHeader().AllowAnyMethod().AllowCredentials().SetIsOriginAllowed(x => true);
        }
    });
});


builder.Services.AddDbContext<OnlineBackendDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("OnlineBackendDbConnection"));
});

#region inject

builder.Services.AddScoped<OnlineBackendDbContext, OnlineBackendDbContext>();

builder.Services.AddControllers();

builder.Services.AddScoped<IOrderServices, OrderServices>();
builder.Services.AddScoped<IExternalOrderService, ExternalOrderService>();
builder.Services.AddScoped<BaseService, BaseService>();
builder.Services.AddScoped<OnlineBackendDbContext, OnlineBackendDbContext>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITradeServices, TradeService>();
builder.Services.AddScoped<IExternalTradeService, ExternalTradeService>();
builder.Services.AddScoped<IExternalUserService, ExternalUserService>();
builder.Services.AddScoped<IInstrumentsService, InstrumentsService>();
builder.Services.AddScoped<IExternalInstrumentService, ExternalInstrumentService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBaseRepository, BaseRepository>();
builder.Services.AddScoped<IInstrumentsRepository, InstrumentsRepository>();
builder.Services.AddScoped<IPushService, PushService>();

#endregion


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();




builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";

});
//var _CacheData = new CacheData(builder);

builder.Services.AddSignalR();



builder.Services.AddControllersWithViews();
builder.Services.Configure<FormOptions>(o =>  // currently all set to max, configure it to your needs!
{
    o.ValueLengthLimit = int.MaxValue;
    o.MultipartBodyLengthLimit = long.MaxValue; // <-- !!! long.MaxValue
    o.MultipartBoundaryLengthLimit = int.MaxValue;
    o.MultipartHeadersCountLimit = int.MaxValue;
    o.MultipartHeadersLengthLimit = int.MaxValue;
});

var app = builder.Build();


//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("CustomCors");

app.UseAuthorization();
app.MapControllers();
app.MapHub<CustomersHub>("/CustomersHub");

app.Run();
//CacheData.
