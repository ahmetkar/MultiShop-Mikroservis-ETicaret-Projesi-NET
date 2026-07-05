using Microsoft.AspNetCore.Authentication.JwtBearer;
using MultiShop.Payment.DAL.Context;
using MultiShop.Payment.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IPaymentService, PaymentService>();

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
              c.Type == "scope" &&
              c.Value.Split(' ').Contains("PaymentReadPermission")));
    });

    options.AddPolicy("PaymentCreatePolicy", policy =>
    {
        policy.RequireAssertion(context =>
          context.User.Claims.Any(c =>
              c.Type == "scope" &&
              c.Value.Split(' ').Contains("PaymentCreatePermission")));
    });

    options.AddPolicy("PaymentUpdatePolicy", policy =>
    {
        policy.RequireAssertion(context =>
        context.User.Claims.Any(c =>
            c.Type == "scope" &&
            c.Value.Split(' ').Contains("PaymentUpdatePermission")));
    });

    options.AddPolicy("PaymentDeletePolicy", policy =>
    {
        policy.RequireAssertion(context =>
        context.User.Claims.Any(c =>
            c.Type == "scope" &&
            c.Value.Split(' ').Contains("PaymentDeletePermission")));
    });
});

builder.Services.AddDbContext<PaymentContext>();


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
