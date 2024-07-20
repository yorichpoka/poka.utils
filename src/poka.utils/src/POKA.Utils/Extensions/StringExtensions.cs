using POKA.Utils.ValueObjects;

namespace POKA.Utils.Extensions
{
    public static class StringExtensions
    {
        public static bool HasValue(this string value) => !value.IsNullOrEmpty() && !value.IsNullOrWhiteSpace();

        public static bool IsNullOrWhiteSpace(this string value) => string.IsNullOrWhiteSpace(value);

        public static bool IsNullOrEmpty(this string value) => string.IsNullOrEmpty(value);

        public static TEnum ToEnum<TEnum>(this string value) where TEnum : Enum
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            return (TEnum)Enum.Parse(typeof(TEnum), value, true);
        }

        public static TObjectId ToObjectId<TObjectId>(this string value) where TObjectId : BaseObjectId
            => BaseObjectId.Parse<TObjectId>(value);
    }
}
