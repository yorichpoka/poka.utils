using POKA.Utils.Infrastructure.SqlServer.EntityTypeConfigurations;

namespace POKA.Utils.Infrastructure.SqlServer.DbContexts
{
    public class SqlEventStoreDbContext : DbContext
    {
        public SqlEventStoreDbContext(DbContextOptions<SqlEventStoreDbContext> options)
            : base(options)
        {
            ChangeTracker.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) =>
            modelBuilder
                .ApplyConfiguration(new RequestEntityTypeConfiguration())
                .ApplyConfiguration(new EventEntityTypeConfiguration());
    }
}
