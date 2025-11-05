using OrderService.Abstractions.Repositories;
using OrderService.Abstractions.Services;
using OrderService.Api.Clients.Users;
using OrderService.Application.Repositories;
using Scalar.AspNetCore;
using OrderSvc = OrderService.Application.Services.OrderService;

using UserService.Abstractions.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();
builder.Services.AddScoped<IUserService>(_ => new UserServiceHttpClient(new HttpClient() { BaseAddress = new Uri("http://127.0.0.1:5217") }));
builder.Services.AddScoped<IOrderService, OrderSvc>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

#if DEBUG
app.MapOpenApi();
app.MapScalarApiReference();
#endif

app.MapControllers();

app.Run();
