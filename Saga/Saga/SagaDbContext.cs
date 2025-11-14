using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Saga.Orchestrators;

namespace Saga;

public class DbContext : SagaDbContext
{
    public DbContext(DbContextOptions<SagaDbContext> options) 
        : base(options)
    {
    }

    protected override IEnumerable<ISagaClassMap> Configurations
    {
        get { yield return new UserCreationOrchestratorMap(); }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("saga");
    }
}

public class UserCreationOrchestratorMap : SagaClassMap<UserCreationOrchestrator>
{
    protected override void Configure(EntityTypeBuilder<UserCreationOrchestrator> entity, ModelBuilder model)
    {
        entity.ToTable("user_creation_sagas");
        entity.Property(x => x.Username)
            .HasMaxLength(100)
            .IsRequired();
        entity.Property(x => x.Password)
            .HasMaxLength(255);
        entity.Property(x => x.FailureReason)
            .HasMaxLength(1000);
        
        entity.HasIndex(x => x.Username);
        entity.HasIndex(x => x.CurrentState);
    }
}