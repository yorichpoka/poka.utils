namespace POKA.Utils.Extensions
{
    public static class EntityTypeBuilderExtensions
    {
        public static EntityTypeBuilder<TEntity> ConfigureBaseEntity<TEntity, TObjectId>(this EntityTypeBuilder<TEntity> builder, string tableName, string schemaName)
            where TEntity : BaseEntity<TObjectId>
            where TObjectId : BaseObjectId
        {
            builder
                .ToTable(tableName, schemaName);

            builder
                .Ignore(nameof(ChangeTracker.PropertyChanged))
                .Ignore(nameof(ChangeTracker.IsTracking))
                .Ignore(nameof(ChangeTracker.IsChanged));

            builder
                .HasKey(e => e.Id);

            builder
                .Property(l => l.Id)
                .HasColumnName("Id")
                .HasConversion(
                    value => value.Value,
                    dbValue => dbValue.ToObjectId<TObjectId>()
                );

            return builder;
        }

        public static EntityTypeBuilder<TEntity> ConfigureAggregate<TEntity, TObjectId>(this EntityTypeBuilder<TEntity> builder, string tableName, string schemaName)
            where TEntity : AggregateRoot<TObjectId>
            where TObjectId : BaseObjectId
        {
            builder
                .ConfigureBaseEntity<TEntity, TObjectId>(tableName, schemaName)
                .ConfigureHasCreatedByUserId()
                .ConfigureHasCreatedOn()
                .ConfigureHasVersion();

            builder
                .Property(l => l.LastUpdatedOn)
                .HasColumnName("LastUpdatedOn")
                .IsRequired();

            return builder;
        }

        public static EntityTypeBuilder<TEntity> ConfigureHasCreatedByUserId<TEntity>(this EntityTypeBuilder<TEntity> builder)
            where TEntity : class, IHasCreatedByUserId
        {
            builder
                .Property(l => l.CreatedByUserId)
                .HasColumnName("CreatedByUserId")
                .HasConversion(
                    value => value.Value,
                    dbValue => new UserId(dbValue)
                )
                .IsRequired(false);

            return builder;
        }

        public static EntityTypeBuilder<TEntity> ConfigureHasCreatedOn<TEntity>(this EntityTypeBuilder<TEntity> builder)
            where TEntity : class, IHasCreatedOn
        {
            builder
                .Property(l => l.CreatedOn)
                .HasColumnName("CreatedOn")
                .ValueGeneratedOnAdd()
                .IsRequired();

            return builder;
        }

        public static EntityTypeBuilder<TEntity> ConfigureHasVersion<TEntity>(this EntityTypeBuilder<TEntity> builder)
            where TEntity : class, IHasVersion
        {
            builder
                .Property(l => l.Version)
                .HasColumnName("Version")
                .IsRequired();

            return builder;
        }
    }
}
