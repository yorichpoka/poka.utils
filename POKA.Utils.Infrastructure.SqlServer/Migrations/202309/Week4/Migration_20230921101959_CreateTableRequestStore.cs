using FluentMigrator;

namespace POKA.Utils.Infrastructure.SqlServer.Migrations._202309.Week4
{
    [Migration(20230921101959)]
    public class Migration_20230921101959_CreateTableRequestStore : AutoReversingMigration
    {
        public override void Up()
        {
            Create
                .Table("RequestStore")
                .InSchema("event")
                    // Primary key
                    .WithColumn("Id")
                        .AsGuid()
                        .NotNullable()
                        .PrimaryKey()
                        .WithDefault(SystemMethods.NewSequentialId)
                    // Not nullable
                    .WithColumn("ApplicationPerformer")
                        .AsString(100)
                        .NotNullable()
                    .WithColumn("Name")
                        .AsString(250)
                        .NotNullable()
                    .WithColumn("Data")
                        .AsString(int.MaxValue)
                        .NotNullable()
                    .WithColumn("Status")
                        .AsString(100)
                        .NotNullable()
                    .WithColumn("Type")
                        .AsString(100)
                        .NotNullable()
                    .WithColumn("CreatedOn")
                        .AsDateTime2()
                        .NotNullable()
                        .WithDefault(SystemMethods.CurrentUTCDateTime)
                    .WithColumn("ScopeId")
                        .AsGuid()
                        .NotNullable()
                    // Nullable
                    .WithColumn("Duration")
                        .AsTime()
                        .Nullable()
                    .WithColumn("Error")
                        .AsString(int.MaxValue)
                        .Nullable()
                    .WithColumn("CreatedByUserId")
                        .AsGuid()
                        .Nullable()
                    .WithColumn("ParentId")
                        .AsGuid()
                        .Nullable()
                        .ForeignKey(
                            foreignKeyName: "FK_RequestStore_RequestStore_Parent",
                            primaryTableName: "RequestStore",
                            primaryTableSchema: "event",
                            primaryColumnName: "Id"
                        );
        }
    }
}
