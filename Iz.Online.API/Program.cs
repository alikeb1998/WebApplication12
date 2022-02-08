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
using Iz.Online.SignalR;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors();

// Add services to the container.
//test

builder.Services.AddDbContext<OnlineBackendDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("OnlineBackendDbConnection"));
});
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

//HttpRequest


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();




builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";

});
//var config = builder.Configuration.GetSection("RestApiConfigs");
//CacheData.RestApiConfigs.Address = config.GetValue<string>("Address");
//CacheData.RestApiConfigs.Authorization = config.GetValue<string>("Authorization");

var _CacheData = new CacheData(builder);

builder.Services.AddSignalR();


builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed(origin => true) // allow any origin
                    .AllowCredentials());

app.UseAuthorization();
app.MapControllers();
app.MapHub<CustomersHub>("/CustomersHub");

app.Run();
