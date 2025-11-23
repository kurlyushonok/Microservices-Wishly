using Api.Consumers;
using Dal;
using Logic;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CoreLib.RedisSync;
using CoreLib.DistributedLockLogic;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

//регистрация зависимостей
builder.Services.AddRedisDistributedSemaphore(
    builder.Configuration.GetConnectionString("Redis"));
builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddBusinessLogic();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<CreateUserConsumer>();
    x.AddConsumer<CompensateUserCreationConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();