using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MultiShop.Message.DAL.Context;
using MultiShop.Message.Mapping;
using MultiShop.Message.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    opt.Authority = builder.Configuration["IdentityServerURL"];
    opt.RequireHttpsMetadata = false;
    opt.Audience = "ResourceMessage";
});


// Add services to the container.

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());


var conStr = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<MessageContext>(opt =>
{
    opt.UseNpgsql(conStr);
});

builder.Services.AddScoped<IUserMessageService, UserMessageService>();


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
