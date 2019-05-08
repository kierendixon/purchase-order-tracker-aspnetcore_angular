using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PurchaseOrderTracker.Domain.Models.PurchaseOrderAggregate;
using PurchaseOrderTracker.Domain.Models.PurchaseOrderAggregate.ValueObjects;
using PurchaseOrderTracker.Domain.Models.ShipmentAggregate;
using PurchaseOrderTracker.Domain.Models.ShipmentAggregate.ValueObjects;
using PurchaseOrderTracker.Domain.Models.SupplierAggregate;

namespace PurchaseOrderTracker.Persistence
{
    // TODO: Use numeric values from domain model instead of duplicating in definitions here
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
            modelBuilder.Entity<Product>(ConfigureProduct);
            modelBuilder.Entity<ProductCategory>(ConfigureProductCategory);
            modelBuilder.Entity<PurchaseOrder>(ConfigurePurchaseOrder);
            modelBuilder.Entity<PurchaseOrderLine>(ConfigurePurchaseOrderLine);
            modelBuilder.Entity<Shipment>(ConfigureShipment);
            modelBuilder.Entity<Supplier>(ConfigureSupplier);
        }

        private void ConfigureProductCategory(EntityTypeBuilder<ProductCategory> entity)
        {
            entity.OwnsOne(p => p.Name, o =>
            {
                o.Property(p => p.Value).IsRequired().HasMaxLength(50);
            });

            // TODO
            //entity.HasIndex(c => new { c.SupplierId, c.Name }).IsUnique();
        }

        private void ConfigurePurchaseOrder(EntityTypeBuilder<PurchaseOrder> entity)
        {
            entity.Property<int>("SupplierId").IsRequired();
            entity.Property<int>("StatusId").IsRequired();
            entity.OwnsOne(p => p.Status, o =>
            {
                o.ToTable(nameof(PurchaseOrderStatus));
            });
            entity.OwnsOne(p => p.OrderNo, o =>
            {
                o.HasIndex(p => p.Value).IsUnique();
                o.Property(p => p.Value).IsRequired().HasMaxLength(20);
            });
        }

        private void ConfigurePurchaseOrderLine(EntityTypeBuilder<PurchaseOrderLine> entity)
        {
            entity.Property(p => p.PurchasePrice).IsRequired();
            entity.Property(p => p.PurchaseQty).IsRequired();
            entity.Property<int>("ProductId").IsRequired();
            entity.Property<int>("PurchaseOrderId").IsRequired();
        }

        private void ConfigureShipment(EntityTypeBuilder<Shipment> entity)
        {
            entity.Property(s => s.DestinationAddress);
            entity.Property(s => s.EstimatedArrivalDate).IsRequired();
            entity.Property<int>("StatusId").IsRequired();
            entity.OwnsOne(p => p.Status, o =>
            {
                o.ToTable(nameof(ShipmentStatus));
            });

            entity.HasIndex(s => s.TrackingId).IsUnique();
            entity.HasIndex(s => s.EstimatedArrivalDate);
        }

        private void ConfigureSupplier(EntityTypeBuilder<Supplier> entity)
        {
            entity.Property(s => s.Id).ForSqlServerUseSequenceHiLo();
            entity.OwnsOne(p => p.Name, o =>
            {
                o.HasIndex(p => p.Value).IsUnique();
                o.Property(p => p.Value).IsRequired().HasMaxLength(20);
            });
        }

        private void ConfigureProduct(EntityTypeBuilder<Product> entity)
        {
            entity.Property(b => b.Id).ForSqlServerUseSequenceHiLo();
            entity.Property(p => p.Price).IsRequired();
            entity.Property<int>("CategoryId").IsRequired();
            entity.OwnsOne(p => p.ProductCode, o =>
            {
                o.HasIndex(p => p.Value).IsUnique();
                o.Property(p => p.Value).IsRequired().HasMaxLength(20);
            });
            entity.OwnsOne(p => p.Name, o =>
            {
                o.Property(p => p.Value).IsRequired().HasMaxLength(150);
            });

            // TODO
            // entity.HasIndex(p => new { p.SupplierId, p.ProdCode }).IsUnique();
            // entity.HasIndex(p => new { p.SupplierId, p.Name }).IsUnique();
        }
    }
}
