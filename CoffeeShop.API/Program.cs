using Microsoft.EntityFrameworkCore;
using CoffeeShop.API.Data;
using CoffeeShop.API.Repositories;
using GraphQL;
using CoffeeShop.API.GraphQL;
using GraphQL.Types;
using GraphQL.Server;
using CoffeeShop.API.GraphQL.Types;
using CoffeeShop.API.GraphQL.Subscriptions;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using Serilog.Exceptions;

var builder = WebApplication.CreateBuilder(args);

var elasticUri = builder.Configuration["ElasticSearch:Url"];
var elasticIndex = builder.Configuration["ElasticSearch:Log_Index"];

builder.Host.UseSerilog();

Log.Logger = new LoggerConfiguration()
    .Filter.ByExcluding(c => c.Properties.Any(p => p.Value.ToString().ToLower().Contains("unhealty")))
    .Filter.ByExcluding(c => c.Properties.Any(p => p.Value.ToString().ToLower().Contains("healty")))
    .Filter.ByExcluding(c => c.Properties.Any(p => p.Value.ToString().ToLower().Contains("degrade")))
    .Filter.ByExcluding(c => c.MessageTemplate.Text.ToLower().Contains("health check"))
    .Filter.ByExcluding(c => c.MessageTemplate.Text.ToLower().Contains("hosting"))
    .Filter.ByExcluding(c => c.MessageTemplate.Text.ToLower().Contains("content"))
    .Filter.ByExcluding(c => c.MessageTemplate.Text.ToLower().Contains("{address}")) 
    .Filter.ByExcluding(c => c.MessageTemplate.Text.ToLower().Contains("ctrl+c"))
    .Filter.ByExcluding(c => c.MessageTemplate.Text.ToLower().Contains("dbcommand"))
    .Filter.ByExcluding(c => c.MessageTemplate.Text.ToLower().Contains("{version}"))
    .WriteTo.Console()
    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticUri))
    {
        RegisterTemplateFailure = RegisterTemplateRecovery.FailSink,
        AutoRegisterTemplate = true,
        AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
        
        IndexDecider = (e, o) =>
        {
            return $"{elasticIndex.ToLower()}-{DateTime.UtcNow:yyyy-MM-dd}";
        },
    }).CreateLogger();


builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer
(builder.Configuration.GetConnectionString("DbConnection")));

builder.Services.AddScoped<IMenuRepository, MenuRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();

builder.Services.AddSingleton<MenuSubscriptionService>();
builder.Services.AddSingleton<ProductSubscriptionService>();
builder.Services.AddSingleton<ReservationSubscriptionService>();

builder.Services.AddScoped<IServiceProvider>(s => new FuncServiceProvider(s.GetRequiredService));
builder.Services.AddScoped<MenuType>();
builder.Services.AddScoped<ProductType>();
builder.Services.AddScoped<ReservationType>();
builder.Services.AddScoped<MenuInputType>();
builder.Services.AddScoped<ProductInputType>();
builder.Services.AddScoped<ReservationInputType>();
builder.Services.AddScoped<Query>();
builder.Services.AddScoped<Mutation>();
builder.Services.AddScoped<Subscription>();
builder.Services.AddScoped<ISchema, CoffeeShopSchema>();

builder.Services.AddCors();
builder.Services.AddGraphQL(options =>
                {
                    options.EnableMetrics = true;
                    options.UnhandledExceptionDelegate = ctx =>
                    {
                        Console.WriteLine("error: " + ctx.OriginalException.Message);
                    };
                })
                .AddWebSockets()
                .AddGraphTypes(typeof(CoffeeShopSchema))
                .AddSystemTextJson();



var app = builder.Build();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    // endpoints.MapGraphQL();
});


// app.UseGraphQLVoyager(new VoyagerOptions(){GraphQLEndPoint = "/graphql"}, "/graphql-voyager");
app.UseWebSockets();
app.UseGraphQLPlayground();
app.UseGraphQL<ISchema>();
app.UseGraphQLWebSockets<ISchema>("/graphql");
app.Run();