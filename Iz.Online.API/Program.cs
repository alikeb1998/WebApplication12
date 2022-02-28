
using Iz.Online.API.Infrastructure;
using Iz.Online.DataAccess;
using Iz.Online.HubHandler.IServices;
using Iz.Online.HubHandler.Services;
using Iz.Online.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Features;
using StackExchange.Redis;
using Iz.Online.Services.IServices;

var builder = WebApplication.CreateBuilder(args);

#region Corse=>

CorsExtensions.AddCustomCors(builder.Services, builder.Configuration);


#endregion

builder.Services.AddDbContext<OnlineBackendDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("OnlineBackendDbConnection"));
});

#region inject

InjectionHandler.InjectServices(builder.Services);
builder.Services.AddScoped<IHubUserService, HubUserService>();

#endregion
//builder.Services.AddDistributedMemoryCache();


//);

builder.Services.AddAuthentication()
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Unauthorized/";
        options.AccessDeniedPath = "/Account/Forbidden/";
    })
    .AddJwtBearer(options =>
    {
        options.Audience = "http://localhost:5001/";
        options.Authority = "http://localhost:5000/";
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region  AddRedis


builder.Services.AddStackExchangeRedisCache(option =>
{
    option.Configuration = "localhost:6379";
    option.InstanceName = "";
});
builder.Services.AddSingleton<IConnectionMultiplexer>(provider => ConnectionMultiplexer.Connect("localhost:6379"));

#endregion


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


var hubService = builder.Services.BuildServiceProvider().GetService<IHubUserService>();
Task.Run(async () => hubService.CreateAllConsumers());

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
