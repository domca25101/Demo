using CoffeeShop.Client.Clients;
using CoffeeShop.Client.GraphQLSubscription;
using CoffeeShop.Client.RabbitMQ;
using EasyNetQ;
using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Serilog;
using Serilog.Sinks.Elasticsearch;

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
    .Filter.ByExcluding(c => c.MessageTemplate.Text.ToLower().Contains("{version}"))
    .Filter.ByExcluding(c => c.MessageTemplate.Text.Contains("Exec"))
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

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var bus = RabbitHutch.CreateBus(builder.Configuration.GetConnectionString("RabbitMQ"), registerServices: s => s.Register<ITypeNameSerializer, TypeNameSerializer>());
builder.Services.AddSingleton(bus);
builder.Services.AddSingleton<IMessagePublisher, MessagePublisher>();

builder.Services.AddSingleton<IGraphQLClient>(p => new GraphQLHttpClient(builder.Configuration.GetConnectionString("CoffeeShopApi"), new NewtonsoftJsonSerializer()));
builder.Services.AddSingleton<MenuGQLClient>();
builder.Services.AddSingleton<ProductGQLClient>();
builder.Services.AddSingleton<ReservationGQLClient>();
builder.Services.AddSingleton<SubscriptionHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using var subscriptionHandler = app.Services.GetService<SubscriptionHandler>().SubscribeAll();

app.Run();