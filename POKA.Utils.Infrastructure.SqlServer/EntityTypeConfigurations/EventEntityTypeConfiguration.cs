namespace POKA.Utils.Infrastructure.SqlServer.EntityTypeConfigurations
{
    public class EventEntityTypeConfiguration : IEntityTypeConfiguration<EventEntity>
    {
        public void Configure(EntityTypeBuilder<EventEntity> builder)
        {
            builder
                .ConfigureBaseEntity<EventEntity, EventId>("EventStore", "dbo")
                .ConfigureHasCreatedOn()
                .ConfigureHasVersion();

            builder
                .Property(l => l.AggregateId)
                .HasColumnName("AggregateId")
                .IsRequired();

            builder
                .Property(l => l.AggregateType)
                .HasColumnName("AggregateType")
                .IsRequired();

            builder
                .Property(l => l.Type)
                .HasColumnName("Type")
                .IsRequired();

            builder
                .Property(l => l.Data)
                .HasColumnName("Data")
                .IsRequired();
        }
    }
}