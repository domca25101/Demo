using Delivery.API.Models;
using Delivery.API.RabbitMQ;
using Delivery.API.RabbitMQ.EventProcessing;
using Delivery.API.Repositories;
using Delivery.API.Services;
using EasyNetQ;
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
    .Filter.ByExcluding(c => c.MessageTemplate.Text.ToLower().Contains("dbcommand"))
    .Filter.ByExcluding(c => c.MessageTemplate.Text.ToLower().Contains("{version}"))
    .Filter.ByExcluding(c => c.MessageTemplate.Text.Contains("broker"))
    .Filter.ByExcluding(c => c.MessageTemplate.Text.Contains("consumer"))
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

builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("Database"));

var bus = RabbitHutch.CreateBus(
    builder.Configuration.GetConnectionString("RabbitMQ"),
    registerServices: s => s.Register<ITypeNameSerializer, TypeNameSerializer>());
builder.Services.AddSingleton(bus);

builder.Services.AddSingleton<MenuRepository>();
builder.Services.AddSingleton<ProductRepository>();
builder.Services.AddSingleton<ReservationRepository>();
builder.Services.AddSingleton<MenuEventProcessor>();
builder.Services.AddSingleton<ProductEventProcessor>();
builder.Services.AddSingleton<ReservationEventProcessor>();

builder.Services.AddHostedService<SubscriptionService>();

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

app.Run();
