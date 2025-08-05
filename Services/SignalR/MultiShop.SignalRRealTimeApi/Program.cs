using MultiShop.SignalRRealTimeApi.Handlers;
using MultiShop.SignalRRealTimeApi.Hubs;
using MultiShop.SignalRRealTimeApi.Services;
using MultiShop.SignalRRealTimeApi.Services.SignalRCommentServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

builder.Services.AddControllers();


builder.Services.AddAccessTokenManagement();
builder.Services.AddHttpContextAccessor();

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed((host) => true).AllowCredentials();
    });
});

builder.Services.AddSignalR();

//builder.Services.AddScoped<ISignalRCommentService,SignalRCommentService>();
builder.Services.AddScoped<ClientCredentialTokenHandler>();

builder.Services.AddHttpClient<IClientCredentialTokenService, ClientCredentialTokenService>();

builder.Services.AddHttpClient<ISignalRCommentService, SignalRCommentService>(opt =>
{
    opt.BaseAddress = new Uri($"http://localhost:5000/services/comment/");
}).AddHttpMessageHandler<ClientCredentialTokenHandler>();




var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");




app.UseAuthentication();
app.UseAuthorization();

app.MapHub<SignalRHub>("/signalrhub");

app.MapControllers();

app.Run();
