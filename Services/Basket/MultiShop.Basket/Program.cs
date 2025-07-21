using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Options;
using MultiShop.Basket.LoginServices;
using MultiShop.Basket.Services;
using MultiShop.Basket.Settings;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);


var requireAuthorizationPolicy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build();
//JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub"); // Remove the default claim type mapping for "sub" claim
//JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
// JwtSecurityTokenHandler.DefaultMapInboundClaims = false; .net 8 de �al��an versiyonu
//sub de�eri burada ayr� bir alan olarak gelmiyor bir liste eleman�n i�indeki listede geliyor.Bunu istemeyiz
//Bunun yerine sub de�erinin tek ba��na gelmesini istiyoruz.
//Bu y�zden Program.cs de DefaultInBoundClaimTypeMap.Remove metodu kullan�lm��tt�r.
//basket controllerde test edersek _> var user = User.Claims;

// Add services to the container.

builder.Services.AddAuthentication(opt=> {
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(opt =>
{
    opt.Authority = builder.Configuration["IdentityServerURL"];
    opt.Audience = "ResourceBasket";
    opt.RequireHttpsMetadata = false;
});


builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IBasketService, BasketService>();
builder.Services.AddScoped<ILoginService,LoginService>();
builder.Services.Configure<RedisSettings>(builder.Configuration.GetSection("RedisSettings"));

builder.Services.AddSingleton<RedisService>(sp =>
{
    var redisSettings = sp.GetRequiredService<IOptions<RedisSettings>>().Value;
    var redis = new RedisService(redisSettings.Host,redisSettings.Port);
    redis.Connect();
    return redis;
});

//Proje seviyesinde authorize i�lemi nas�l yap�l�r ?

builder.Services.AddControllers(opt =>
{
    opt.Filters.Add(new AuthorizeFilter(requireAuthorizationPolicy));
});


builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
