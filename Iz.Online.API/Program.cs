
using Iz.Online.API.Infrastructure;
using Iz.Online.DataAccess;
using Iz.Online.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Features;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);


#region Corse=>

CorsExtensions.AddCustomCors(builder.Services, builder.Configuration);

//var corsOrigins = new List<string>()
//{
//    "http://192.168.72.112:4444/" , "http://localhost:4444/" , "http://127.0.0.1:4444/",
//    "http://192.168.72.112:5555/" , "http://localhost:5555/" , "http://127.0.0.1:5555/",

//    "http://192.168.72.112:4444" , "http://localhost:4444" , "http://127.0.0.1:4444",
//    "http://192.168.72.112:5555" , "http://localhost:5555" , "http://127.0.0.1:5555",

//    "http://localhost:3000" , "http://localhost:3000"
//};

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("CustomCors", policy =>
//    {
//        policy.SetIsOriginAllowedToAllowWildcardSubdomains();

//        foreach (string item in corsOrigins)
//        {
//            policy.WithOrigins(item).AllowAnyHeader().AllowAnyMethod().AllowCredentials().SetIsOriginAllowed(x => true);
//        }
//    });
//});

#endregion

builder.Services.AddDbContext<OnlineBackendDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("OnlineBackendDbConnection"));
});

#region inject

InjectionHandler.InjectServices(builder.Services);

#endregion

//var usermanager = builder.Services.BuildServiceProvider().GetService<IUserService>();
//var t  = usermanager.AllAssets();

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
