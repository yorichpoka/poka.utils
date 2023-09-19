using POKA.Utils.Interfaces;

namespace POKA.Utils.Extensions
{
    public static class IObjectIdExtensions
    {
        public static bool HasValue(this IObjectId value) => (value?.Value ?? Guid.Empty) != Guid.Empty;
    }
}
