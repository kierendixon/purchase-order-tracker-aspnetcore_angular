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

            modelBuilder.Entity<Product>(ConfigureProduct);
            modelBuilder.Entity<ProductCategory>(ConfigureProductCategory);
            modelBuilder.Entity<PurchaseOrder>(ConfigurePurchaseOrder);
            modelBuilder.Entity<PurchaseOrderLine>(ConfigurePurchaseOrderLine);
            modelBuilder.Entity<Shipment>(ConfigureShipment);
            modelBuilder.Entity<Supplier>(ConfigureSupplier);
        }

        private void ConfigurePurchaseOrderStatus(EntityTypeBuilder<PurchaseOrderStatus> entity)
        {
            entity.Property<int>("Id").IsRequired();
            entity.HasKey("Id");
        }

        private void ConfigureShipmentStatus(EntityTypeBuilder<ShipmentStatus> entity)
        {
            entity.Property<int>("Id").IsRequired();
            entity.HasKey("Id");
        }

        private void ConfigureProductCategory(EntityTypeBuilder<ProductCategory> entity)
        {
            entity.Property(c => c.Name).IsRequired();

            entity.HasIndex(c => new {c.SupplierId, c.Name}).IsUnique();
        }

        private void ConfigurePurchaseOrder(EntityTypeBuilder<PurchaseOrder> entity)
        {
            entity.Property(p => p.OrderNo).IsRequired();
            entity.Property<int>("SupplierId").IsRequired();
            entity.Property<int>("StatusId").IsRequired();

            entity.HasIndex(p => p.OrderNo).IsUnique();
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
            entity.Property(s => s.DestinationAddress).IsRequired();
            entity.Property(s => s.EstimatedArrivalDate).IsRequired();
            entity.Property<int>("StatusId").IsRequired();

            entity.HasIndex(s => s.TrackingId).IsUnique();
            entity.HasIndex(s => s.EstimatedArrivalDate);
        }

        private void ConfigureSupplier(EntityTypeBuilder<Supplier> entity)
        {
            entity.Property(s => s.Id).ForSqlServerUseSequenceHiLo();
            entity.Property(s => s.Name).IsRequired();

            entity.HasIndex(s => s.Name).IsUnique();
        }

        private void ConfigureProduct(EntityTypeBuilder<Product> entity)
        {
            entity.Property(b => b.Id).ForSqlServerUseSequenceHiLo();
            entity.Property(p => p.Name).IsRequired();
            entity.Property(p => p.ProdCode).IsRequired();
            entity.Property(p => p.Price).IsRequired();
            entity.Property<int>("CategoryId").IsRequired();

            entity.HasIndex(p => new { p.SupplierId, p.ProdCode }).IsUnique();
            entity.HasIndex(p => new { p.SupplierId, p.Name }).IsUnique();
        }
    }
}