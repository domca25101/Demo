using Microsoft.EntityFrameworkCore;
using CoffeeShop.API.Data;
using CoffeeShop.API.Repositories;
using GraphQL;
using CoffeeShop.API.GraphQL;
using GraphQL.Types;
using GraphQL.Server;
using CoffeeShop.API.GraphQL.Types;
using GraphiQl;
using CoffeeShop.API.GraphQL.Subscribing;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer
(builder.Configuration.GetConnectionString("DbConnection")));

builder.Services.AddScoped<IMenuRepository, MenuRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();

builder.Services.AddSingleton<SubscriptionService>();

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

builder.Services.AddGraphQL().AddSystemTextJson().AddWebSockets();



var app = builder.Build();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    // endpoints.MapGraphQL();
});

// app.UseGraphQLVoyager(new VoyagerOptions(){GraphQLEndPoint = "/graphql"}, "/graphql-voyager");
app.UseWebSockets();
app.UseGraphiQl("/graphql");
app.UseGraphQL<ISchema>();
app.UseGraphQLWebSockets<ISchema>("/graphql");
app.Run();
