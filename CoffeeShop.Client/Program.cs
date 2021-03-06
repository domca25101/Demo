using CoffeeShop.Client.Clients;
using CoffeeShop.Client.GraphQLSubscription;
using CoffeeShop.Client.RabbitMQ;
using EasyNetQ;
using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddSingleton<GraphQLClient>();
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

var subscriptionHandler = app.Services.GetService<SubscriptionHandler>().SubscribeAll();

app.Run();

subscriptionHandler.Dispose();