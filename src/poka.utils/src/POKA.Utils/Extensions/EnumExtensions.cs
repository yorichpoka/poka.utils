namespace POKA.Utils.Extensions
{
    /// <summary>
    /// Extension class of <see cref="System.Enum"/>.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Get description of enum value.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        public static string GetDescription<TEnum>(this TEnum value) where TEnum : Enum
        {
            var fieldInfo = value.GetType()
                                 .GetField($"{value}");
            var descriptionAttribute = fieldInfo.GetCustomAttribute<DescriptionAttribute>();

            if (descriptionAttribute is null)
            {
                return $"{value}";
            }

            return descriptionAttribute.Description;
        }
    }
}
