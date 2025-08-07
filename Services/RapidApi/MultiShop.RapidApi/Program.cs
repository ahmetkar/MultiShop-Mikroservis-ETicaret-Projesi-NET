var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
<<<<<<< HEAD
//builder.Services.AddControllersWithViews();
=======
builder.Services.AddControllersWithViews();
>>>>>>> 0f8340580aaa225ba29a808fa7e161885441e0cf

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

<<<<<<< HEAD

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllers();
/*
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();
*/
app.UseHttpsRedirection();




=======
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseRouting();
>>>>>>> 0f8340580aaa225ba29a808fa7e161885441e0cf

app.Run();
