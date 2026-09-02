var builder = DistributedApplication.CreateBuilder(args);

var identityServer = builder.AddProject<Projects.MultiShop_IdentityServer>("identityserver").WithHttpEndpoint(
        port: 5001,
        name: "SelfHost")
    .WithEndpoint("SelfHost", endpoint =>
    {
        endpoint.IsProxied = false;
    });

var catalog = builder.AddProject<Projects.MultiShop_Catalog>("catalog").WithHttpEndpoint(
        port: 7070,
        name: "http")
    .WithEndpoint("http", endpoint =>
    {
        endpoint.IsProxied = false;
    });

var discount = builder.AddProject<Projects.MultiShop_Discount>("discount").WithHttpEndpoint(
        port: 7071,
        name: "http")
    .WithEndpoint("http", endpoint =>
    {
        endpoint.IsProxied = false;
    });

var order = builder.AddProject<Projects.MultiShop_Order_API>("order").WithHttpEndpoint(
        port: 7072,
        name: "http")
    .WithEndpoint("http", endpoint =>
    {
        endpoint.IsProxied = false;
    });

var cargo = builder.AddProject<Projects.MultiShop_Cargo_WebApi>("cargo").WithHttpEndpoint(
        port: 7073,
        name: "http")
    .WithEndpoint("http", endpoint =>
    {
        endpoint.IsProxied = false;
    });

var basket = builder.AddProject<Projects.MultiShop_Basket>("basket").WithHttpEndpoint(
        port: 7074,
        name: "http")
    .WithEndpoint("http", endpoint =>
    {
        endpoint.IsProxied = false;
    });

var comment = builder.AddProject<Projects.MultiShop_Comment>("comment").WithHttpEndpoint(
        port: 7085,
        name: "http")
    .WithEndpoint("http", endpoint =>
    {
        endpoint.IsProxied = false;
    });

var payment = builder.AddProject<Projects.MultiShop_Payment>("payment").WithHttpEndpoint(
        port: 7076,
        name: "http")
    .WithEndpoint("http", endpoint =>
    {
        endpoint.IsProxied = false;
    });

var ocelot = builder.AddProject<Projects.MultiShop_OcelotGateway>("ocelotgateway")
    .WithReference(catalog)
    .WithReference(discount)
    .WithReference(order)
    .WithReference(cargo)
    .WithReference(basket)
    .WithReference(comment)
    .WithReference(payment).WithHttpEndpoint(
        port: 5000,
        name: "http")
    .WithEndpoint("http", endpoint =>
    {
        endpoint.IsProxied = false;
    });

var webUI = builder.AddProject<Projects.MultiShop_WebUI>("webui")
    .WithReference(ocelot)
    .WithReference(identityServer).WithHttpEndpoint(
        port: 7021,
        name: "http")
    .WithEndpoint("http", endpoint =>
    {
        endpoint.IsProxied = false;
    });

builder.Build().Run();
