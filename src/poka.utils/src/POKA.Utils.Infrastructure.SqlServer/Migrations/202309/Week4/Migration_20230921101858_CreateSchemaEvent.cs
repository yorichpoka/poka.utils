using FluentMigrator;

namespace POKA.Utils.Infrastructure.SqlServer.Migrations._202309.Week4
{
    [Migration(20230921101858)]
    public class Migration_20230921101858_CreateSchemaEvent : AutoReversingMigration
    {
        public override void Up()
        {
            Create
                .Schema("event");
        }
    }
}
