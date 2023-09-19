using POKA.Utils.Interfaces;

namespace POKA.Utils.ValueObjects
{
    public abstract record BaseObjectId : IObjectId
    {
        protected virtual string _type { get; } = null!;

        public Guid Value { get; private set; }

        public BaseObjectId(Guid value)
        {
            Value = value;
        }

        public static TObjectId Parse<TObjectId>(string value) where TObjectId : BaseObjectId, IObjectId
        {
            try
            {
                var valueParts = value.Split('_');
                var type = valueParts[0];
                var guidValue = Guid.Parse(valueParts[1]);

                var result = (TObjectId)Activator.CreateInstance(typeof(TObjectId), guidValue);

                if (type.ToLower().Trim() != result._type.ToLower())
                {
                    throw new InvalidCastException(value);
                }

                return result;
            }
            catch
            {
                throw new InvalidCastException(value);
            }
        }

        public static bool Is<TObjectId>(string value) where TObjectId : BaseObjectId, IObjectId
        {
            try
            {
                var result = Parse<TObjectId>(value);

                return true;
            }
            catch { }

            return false;
        }

        public static TObjectId Create<TObjectId>(Guid? fromValue = null) where TObjectId : class, IObjectId =>
            Activator.CreateInstance(typeof(TObjectId), fromValue ?? Guid.NewGuid()) as TObjectId;

        public override string ToString() => $"{this._type}_{this.Value}";
    }
}
