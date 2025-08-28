using Microsoft.EntityFrameworkCore;
using orders.domain.Entities;
using orders.domain.Enums;

namespace orders.infrastructure.Data
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Orders");
                entity.HasKey(o => o.OrderId);
                entity.Property(o => o.UserId).IsRequired();
                entity.Property(o => o.Status)
                    .HasConversion(
                        v => v.ToString(),
                        v => (OrderStatus)Enum.Parse(typeof(OrderStatus), v))
                    .IsRequired();

                entity.OwnsMany(o => o.Items, item =>
                {
                    item.ToTable("OrderItems");
                    item.WithOwner().HasForeignKey("OrderId");
                    item.Property(i => i.ProductId).IsRequired();
                    item.Property(i => i.Quantity).IsRequired();
                    item.Property(i => i.UnitPrice).IsRequired().HasColumnType("decimal(18,2)");
                });
            });
        }
    }
}
