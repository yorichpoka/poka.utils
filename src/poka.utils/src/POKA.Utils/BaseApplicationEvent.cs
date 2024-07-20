using POKA.Utils.ValueObjects;

namespace POKA.Utils
{
    public abstract class BaseApplicationEvent<TObjectId> : BaseDomainEvent<TObjectId>
        where TObjectId : BaseObjectId
    {
        protected BaseApplicationEvent(TObjectId id, int version, UserId? authorId = null)
            : base(id, version, authorId)
        {
        }

        protected BaseApplicationEvent(TObjectId id, UserId? authorId = null)
            : base(id, 0, authorId)
        {
        }

        protected BaseApplicationEvent(TObjectId id)
            : base(id, 0, null)
        {
        }
    }
}
