using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PurchaseOrderTracker.Domain.Models.PurchaseOrderAggregate;
using PurchaseOrderTracker.Domain.Models.ShipmentAggregate;
using PurchaseOrderTracker.Domain.Models.SupplierAggregate;

namespace PurchaseOrderTracker.DAL
{
    public class PoTrackerDbContext : DbContext
    {
        public PoTrackerDbContext(DbContextOptions<PoTrackerDbContext> options)
            : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public DbSet<Shipment> Shipment { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrder { get; set; }
        public DbSet<Supplier> Supplier { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductCategory> ProductCategory { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Because EF Core 1 doesn't support mapping complex types / value objects, they are instead
            // stored as a separate table with their primary key implemented as a "shadow property".
            // Support for these mappings will be provided in EF Core 2.
            // See https://github.com/aspnet/EntityFramework/issues/246
            modelBuilder.Entity<PurchaseOrderStatus>(ConfigurePurchaseOrderStatus);
            modelBuilder.Entity<ShipmentStatus>(ConfigureShipmentStatus);

            modelBuilder.Entity<PurchaseOrder>(ConfigurePurchaseOrder);
            modelBuilder.Entity<Supplier>(ConfigureSupplier);
            modelBuilder.Entity<Product>(ConfigureProduct);
            modelBuilder.Entity<ProductCategory>(ConfigureProductCategory);
        }

        private void ConfigurePurchaseOrderStatus(EntityTypeBuilder<PurchaseOrderStatus> entity)
        {
            entity.Property<int>("Id")
                .IsRequired();
            entity.HasKey("Id");
        }

        private void ConfigureShipmentStatus(EntityTypeBuilder<ShipmentStatus> entity)
        {
            entity.Property<int>("Id")
                .IsRequired();
            entity.HasKey("Id");

        }

        private void ConfigureProductCategory(EntityTypeBuilder<ProductCategory> entity)
        {
            entity.HasIndex(c => new {c.SupplierId, c.Name})
                .IsUnique();
        }

        private void ConfigurePurchaseOrder(EntityTypeBuilder<PurchaseOrder> entity)
        {
        }

        private void ConfigureSupplier(EntityTypeBuilder<Supplier> entity)
        {
            entity.Property(b => b.Id)
                .ForSqlServerUseSequenceHiLo();

            entity.Property(s => s.Name).IsRequired();
            entity.HasIndex(s => s.Name)
                .IsUnique();
        }

        private void ConfigureProduct(EntityTypeBuilder<Product> entity)
        {
            entity.Property(b => b.Id)
                .ForSqlServerUseSequenceHiLo();
        }
    }
}