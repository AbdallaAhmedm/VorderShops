using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Vorder.Domain.Entities;

namespace Vorder.Infrastructure.Data
{
    public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options)
    {


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseProperties).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "x");
                    var property = Expression.Property(parameter, nameof(BaseProperties.IsDeleted));
                    var compare = Expression.NotEqual(property, Expression.Constant(true));
                    var lambda = Expression.Lambda(compare, parameter);

                    entityType.SetQueryFilter(lambda);
                }
            }

            modelBuilder.Entity<Shop>(entity =>
            {
                entity.HasKey(e => e.ShopId);
                entity.Property(e => e.ShopName).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.ShopName).IsUnique();
                entity.HasOne(e => e.Owner)
                      .WithMany()
                      .HasForeignKey(e => e.OwnerId)
                      .OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(e => e.Creator)
                      .WithMany()
                      .HasForeignKey(e => e.CreatorId)
                      .OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(e => e.LastModifier)
                      .WithMany()
                      .HasForeignKey(e => e.LastModifierId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Shop>(entity =>
            {
                
            });

            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.HasOne(e => e.Shop)
                      .WithMany()
                      .HasForeignKey(e => e.ShopID)
                      .OnDelete(DeleteBehavior.NoAction);
            });
        }

        public DbSet<Shop> Shops { get; set; }

    }

}
