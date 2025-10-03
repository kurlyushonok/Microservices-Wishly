using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using CoreLib.Entities;

namespace Infrastucture.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options)
    {
    }
    
    public DbSet<Wishlist> Wishlists => Set<Wishlist>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Wishlist>().ToTable("wishlists");
        
        modelBuilder.Entity<Wishlist>(entity =>
        {
            entity.ToTable("wishlists");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .IsRequired();
            
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Description)
                .HasMaxLength(500);
            
            entity.HasOne<BaseEntityDal<Guid>>()
                .WithMany()
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}