using POKA.Utils.ValueObjects;

namespace POKA.Utils.Interfaces
{
    public interface IDomainEvent<TObjectId> : INotification, IHasVersion
        where TObjectId : BaseObjectId
    {
        void AssignId(TObjectId id);
        string GetStreamName();
        DateTime On { get; }
    }
}
