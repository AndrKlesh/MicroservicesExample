using Scalar.AspNetCore;
using UserService.Abstractions.Repositories;
using UserService.Abstractions.Services;
using UserService.Application.Repositories;
using UserSvc = UserService.Application.Services.UserService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IUserRepository, InMemoryUserRepository>();
builder.Services.AddScoped<IUserService, UserSvc>();


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
