namespace POKA.Utils.Infrastructure.SqlServer.DbContexts
{
    public partial class SqlMasterDbContext : DbContext
    {
        public SqlMasterDbContext(DbContextOptions<SqlMasterDbContext> options)
            : base(options)
        {
        }
    }
}
