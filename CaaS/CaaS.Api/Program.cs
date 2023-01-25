using CaaS.Core;
using CaaS.Core.Interfaces;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services, builder.Configuration, builder.Environment);

var app = builder.Build();

ConfigureMiddleware(app, app.Environment);
ConfigureEndpoints(app);

app.Run();

// Add service to container
void ConfigureServices(IServiceCollection services,
    IConfiguration configuration,
    IHostEnvironment evn)
{
    builder.Services.AddControllers(options => options.ReturnHttpNotAcceptable = true)
                    .AddNewtonsoftJson()
                    .AddXmlDataContractSerializerFormatters();

    services.AddSingleton<IProductManagementLogic, ProductManagementLogic>();
    services.AddSingleton<ICartManagementLogic, CartManagementLogic>();
    services.AddSingleton<IDiscountManagementLogic, DiscountManagementLogic>();
    services.AddSingleton<IOrderManagementLogic, OrderManagementLogic>();
    services.AddSingleton<IAnalyticsManagementLogic, AnalyticsManagementLogic>();
    services.AddSingleton<ICustomerManagementLogic, CustomerManagementLogic>();
    services.AddSingleton<IShopManagementLogic, ShopManagementLogic>();
    services.AddSingleton<IShopOwnerManagement, ShopOwnerManagementLogic>();

    // service.AddScoped // per request
    // service.AddTransient // immer neu pro referenz

    services.AddRouting(options => options.LowercaseUrls = true);

    services.AddAutoMapper(typeof(Program));

    services.AddOpenApiDocument(settings =>
            settings.PostProcess = doc => doc.Info.Title = "CaaS API");

    services.AddCors(options => options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()));
    //services.AddHostedService<QueueUpdateService>();
    //services.AddSingleton<UpdateChannel>();
}

// Configure the HTTP request pipeline.
void ConfigureMiddleware(IApplicationBuilder app, IHostEnvironment env)
{
    app.UseCors();

    app.UseHttpsRedirection();
    app.UseAuthorization();

    app.UseOpenApi();
    app.UseSwaggerUi3(settings => settings.Path = "/swagger");
    app.UseReDoc(settings => settings.Path = "/redoc");
}

// Configure the routing system
void ConfigureEndpoints(IEndpointRouteBuilder app)
{
    app.MapControllers();
}