namespace POKA.Utils
{
    public abstract class BaseEnum<TEnum> : IEquatable<BaseEnum<TEnum>> where TEnum : BaseEnum<TEnum>
    {
        private static readonly Dictionary<int, TEnum> _enumerations = CreateEnumerations();

        public int Value { get; protected init; }
        public string Name { get; protected init; } = string.Empty;

        protected BaseEnum(int value, string name)
        {
            Value = value;
            Name = name;
        }

        public static TEnum FromValue(int value)
        {
            var result = _enumerations.TryGetValue(value, out TEnum? enumValue)
                            ? enumValue
                            : default;

            if (result == default)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value.ToString());
            }

            return result;
        }

        public static TEnum FromValue(string name)
        {
            var result = _enumerations.Values.FirstOrDefault(l => l.Name.ToLower().Trim() == name.ToLower().Trim());

            if (result == default)
            {
                throw new ArgumentOutOfRangeException(typeof(TEnum).Name, name);
            }

            return result;
        }

        public static Dictionary<int, TEnum> CreateEnumerations()
        {
            var type = typeof(TEnum);
            var fields = type
                            .GetFields(
                                BindingFlags.Public |
                                BindingFlags.Static |
                                BindingFlags.FlattenHierarchy
                            )
                            .Where(l => type.IsAssignableFrom(l.FieldType))
                            .Select(l => (TEnum)l.GetValue(default)!);

            return fields.ToDictionary(l => l.Value);
        }

        public bool Equals(BaseEnum<TEnum>? other)
        {
            if (other == null)
            {
                return false;
            }

            var isEqual = GetType() == other.GetType() && Value == other.Value;

            return isEqual;
        }

        public override bool Equals(object? obj)
        {
            var isEqual = obj is BaseEnum<TEnum> other && Equals(other);

            return isEqual;
        }

        public override int GetHashCode() => base.GetHashCode();

        public override string ToString() => $"({Value}) {Name}";
    }
}
