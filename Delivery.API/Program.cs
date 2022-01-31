using Delivery.API.Models;
using Delivery.API.RabbitMQ;
using Delivery.API.RabbitMQ.EventProcessing;
using Delivery.API.Services;
using EasyNetQ;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("Database"));

builder.Services.AddScoped<MenuService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<ReservationService>();

var bus = RabbitHutch.CreateBus(
    builder.Configuration.GetConnectionString("RabbitMQ"),
    registerServices: s => s.Register<ITypeNameSerializer, TypeNameSerializer>());
builder.Services.AddSingleton(bus);

builder.Services.AddSingleton<IEventProcessor, EventProcessor>();
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
