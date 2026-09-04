using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using MultiShop.Payment.Consumers;
using MultiShop.Payment.DAL.Context;
using MultiShop.Payment.Services;
using MultiShop.SharedLayer.Kafka;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IKafkaProducer, KafkaProducer>();

builder.Services.AddHostedService<OrderCreatedConsumer>();
builder.Services.AddHostedService<CargoFailedConsumer>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    opt.Authority = builder.Configuration["IdentityServerURL"];
    opt.RequireHttpsMetadata = false;
    opt.Audience = "ResourcePayment";
});


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("PaymentReadPolicy", policy =>
    {
        policy.RequireAssertion(context =>
          context.User.Claims.Any(c =>
              (c.Type == "scope" || c.Type == "http://schemas.microsoft.com/identity/claims/scope") &&
              (c.Value.Split(' ').Contains("PaymentReadPermission") || c.Value.Split(' ').Contains("PaymentFullPermission"))));
    });

    options.AddPolicy("PaymentCreatePolicy", policy =>
    {
        policy.RequireAssertion(context =>
          context.User.Claims.Any(c =>
              (c.Type == "scope" || c.Type == "http://schemas.microsoft.com/identity/claims/scope") &&
              (c.Value.Split(' ').Contains("PaymentCreatePermission") || c.Value.Split(' ').Contains("PaymentFullPermission"))));
    });

    options.AddPolicy("PaymentUpdatePolicy", policy =>
    {
        policy.RequireAssertion(context =>
          context.User.Claims.Any(c =>
              (c.Type == "scope" || c.Type == "http://schemas.microsoft.com/identity/claims/scope") &&
              (c.Value.Split(' ').Contains("PaymentUpdatePermission") || c.Value.Split(' ').Contains("PaymentFullPermission"))));
    });

    options.AddPolicy("PaymentDeletePolicy", policy =>
    {
        policy.RequireAssertion(context =>
          context.User.Claims.Any(c =>
              (c.Type == "scope" || c.Type == "http://schemas.microsoft.com/identity/claims/scope") &&
              (c.Value.Split(' ').Contains("PaymentDeletePermission") || c.Value.Split(' ').Contains("PaymentFullPermission"))));
    });
});

builder.Services.AddDbContext<PaymentContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
