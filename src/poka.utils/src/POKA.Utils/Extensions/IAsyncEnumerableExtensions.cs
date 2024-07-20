namespace POKA.Utils.Extensions
{
    public static class IAsyncEnumerableExtensions
    {
        public static async Task<List<T>> ToListAsync<T>(this IAsyncEnumerable<T> value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var result = new List<T>();

            await foreach (var item in value)
            {
                result.Add(item);
            }

            return result;
        }
    }
}
