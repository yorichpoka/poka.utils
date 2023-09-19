using POKA.Utils.Interfaces;

namespace POKA.Utils
{
    public abstract class BaseEntity<TObjectId> : ChangeTracker, IEntity
        where TObjectId : class, IObjectId
    {
        public TObjectId Id { get; protected set; } = null!;

        protected BaseEntity()
        {
        }

        public override bool Equals(object obj)
        {
            var compareTo = obj as BaseEntity<TObjectId>;

            if (ReferenceEquals(this, compareTo))
            {
                return true;
            }

            if (ReferenceEquals(null, compareTo))
            {
                return false;
            }

            return Id.Equals(compareTo.Id);
        }

        public static bool operator ==(BaseEntity<TObjectId> a, BaseEntity<TObjectId> b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
            {
                return true;
            }

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(BaseEntity<TObjectId> a, BaseEntity<TObjectId> b) => !(a == b);

        public override int GetHashCode() =>
            (GetType().GetHashCode() * 907) + Id.GetHashCode();

        public override string ToString() =>
            $"{GetType().Name} [Id={Id}]";
    }
}