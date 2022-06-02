using Microsoft.EntityFrameworkCore;
using PurchaseOrderTracker.Domain.Models.IdentityAggregate;
using System;

// TODO namespace & duplicate code
// TODO probably don't need to use EntityFramework for this database because not much data is required
// and only a read operation is performed
namespace PurchaseOrderTracker.Persistence
{
    // based on https://github.com/dotnet/aspnetcore/blob/main/src/Identity/EntityFrameworkCore/src/IdentityDbContext.cs
    public class IdentityDbContext : DbContext
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
            : base(options) { }

        // TODO
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // var storeOptions = GetStoreOptions();
            //var maxKeyLength = 0; // storeOptions?.MaxLengthForKeys ?? 0;
            //var encryptPersonalData = false; // storeOptions?.ProtectPersonalData ?? false;
            // PersonalDataConverter converter = null;
            modelBuilder.Entity<ApplicationUser>(b =>
            {
                b.HasKey(u => u.Id);
                b.HasIndex(u => u.NormalizedUserName).HasDatabaseName("UserNameIndex").IsUnique();
                b.ToTable("AspNetUsers");
                b.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();

                b.Property(u => u.UserName).HasMaxLength(256);
                b.Property(u => u.NormalizedUserName).HasMaxLength(256);

                // TODO
                // if (encryptPersonalData)
                // {
                //    converter = new PersonalDataConverter(this.GetService<IPersonalDataProtector>());
                //    var personalDataProps = typeof(ApplicationUser).GetProperties().Where(
                //                    prop => Attribute.IsDefined(prop, typeof(ProtectedPersonalDataAttribute)));
                //    foreach (var p in personalDataProps)
                //    {
                //        if (p.PropertyType != typeof(string))
                //        {
                //            throw new InvalidOperationException(Resources.CanOnlyProtectStrings);
                //        }
                //        b.Property(typeof(string), p.Name).HasConversion(converter);
                //    }
                // }
            });

        }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of Users.
        /// </summary>
        public DbSet<ApplicationUser> Users { get; set; }

        // private StoreOptions GetStoreOptions() => this.GetService<IDbContextOptions>()
        //                    .Extensions.OfType<CoreOptionsExtension>()
        //                    .FirstOrDefault()?.ApplicationServiceProvider
        //                    ?.GetService<IOptions<IdentityOptions>>()
        //                    ?.Value?.Stores;

        // private class PersonalDataConverter : ValueConverter<string, string>
        // {
        //    public PersonalDataConverter(IPersonalDataProtector protector) : base(s => protector.Protect(s), s => protector.Unprotect(s), default)
        //    { }
        // }
    }
}
