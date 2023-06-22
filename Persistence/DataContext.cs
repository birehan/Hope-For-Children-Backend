using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Domain;
using Domain.Common;

namespace Persistence
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions<DataContext> options)
         : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);


            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);

            // Configure one-to-many relationship between Task and CheckList


            // the relationship between Staff and Photo
            modelBuilder.Entity<Staff>()
                .HasOne(s => s.Photo)
                .WithOne(x => x.Staff)
                .HasForeignKey<Photo>(s => s.StaffId)
                .OnDelete(DeleteBehavior.Cascade);






        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {

            foreach (var entry in ChangeTracker.Entries<BaseDomainEntity>())
            {
                entry.Entity.LastModifiedDate = DateTime.Now;

                if (entry.State == EntityState.Added)
                {
                    entry.Entity.DateCreated = DateTime.Now;
                }
            }


            return base.SaveChangesAsync(cancellationToken);
        }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Photo> Photos { get; set; }

    }
}