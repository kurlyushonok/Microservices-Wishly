using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Saga.Orchestrators;
using Saga.Coordinators;
using Saga.Options;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<RabbitMqOptions>(
    builder.Configuration.GetSection("RabbitMq"));

builder.Services.AddDbContext<SagaDbContext>((provider, options) =>
{
    var connectionString = builder.Configuration.GetConnectionString("SagaDatabase");
    
    options.UseNpgsql(connectionString);
});

builder.Services.AddMassTransit(x =>
{
    x.AddSagaStateMachine<UserCreationOrchestratorStateMachine, UserCreationOrchestrator>()
        .EntityFrameworkRepository(r =>
        {
            r.ConcurrencyMode = ConcurrencyMode.Pessimistic;
            r.ExistingDbContext<SagaDbContext>();
        });
    
    x.AddConsumer<UserCreationCoordinator>();
    
    x.UsingRabbitMq((context, cfg) =>
    {
        var options = context.GetRequiredService<IOptions<RabbitMqOptions>>().Value;
        
        cfg.Host(options.Host, "/", h =>
        {
            h.Username(options.Username);
            h.Password(options.Password);
        });

        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddHealthChecks();

var host = builder.Build();

using (var scope = host.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<SagaDbContext>();
    await dbContext.Database.MigrateAsync();
}

await host.RunAsync();