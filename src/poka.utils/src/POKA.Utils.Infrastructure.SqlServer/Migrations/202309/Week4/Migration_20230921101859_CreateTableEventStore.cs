using FluentMigrator;

namespace POKA.Utils.Infrastructure.SqlServer.Migrations._202309.Week4
{
    [Migration(20230921101859)]
    public class Migration_20230921101859_CreateTableEventStore : AutoReversingMigration
    {
        public override void Up()
        {
            Create
                .Table("EventStore")
                .InSchema("event")
                    // Primary key
                    .WithColumn("Id")
                        .AsGuid()
                        .NotNullable()
                        .PrimaryKey()
                        .WithDefault(SystemMethods.NewSequentialId)
                    // Not nullable
                    .WithColumn("Type")
                        .AsString(250)
                        .NotNullable()
                    .WithColumn("AggregateId")
                        .AsString(250)
                        .NotNullable()
                    .WithColumn("AggregateType")
                        .AsString(250)
                        .NotNullable()
                    .WithColumn("Data")
                        .AsString(int.MaxValue)
                        .NotNullable()
                    .WithColumn("Version")
                        .AsInt32()
                        .NotNullable()
                    .WithColumn("CreatedOn")
                        .AsDateTime2()
                        .NotNullable()
                        .WithDefault(SystemMethods.CurrentUTCDateTime);
        }
    }
}
