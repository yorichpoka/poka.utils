namespace POKA.Utils.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<TResponse> PostAsync<TResponse>(this HttpClient httpClient, string? requestUri, object? data = null, CancellationToken cancellationToken = default)
        {
            var httpResponse = await httpClient.PostAsync(requestUri, data.ToStringContent(), cancellationToken);
            var httpResponseContent = await httpResponse.Content.ReadAsStringAsync(cancellationToken);

            httpResponse.EnsureSuccessStatusCode();

            var result = JsonConvert.DeserializeObject<TResponse>(httpResponseContent);

            return result;
        }


        public static async Task<TResponse> GetAsync<TResponse>(this HttpClient httpClient, string? requestUri, object query = null, CancellationToken cancellationToken = default)
        {
            if (query is not null)
            {
                var properties = from property in query.GetType()
                                                       .GetProperties()
                                 where property.GetValue(query, null) != null
                                 select property.Name + "=" + HttpUtility.UrlEncode(property.GetValue(query, null).ToString());
                requestUri += string.Join("&", properties.ToArray());
            }

            var httpResponse = await httpClient.GetAsync(requestUri, cancellationToken);
            var httpResponseContent = await httpResponse.Content.ReadAsStringAsync(cancellationToken);

            httpResponse.EnsureSuccessStatusCode();

            var result = JsonConvert.DeserializeObject<TResponse>(httpResponseContent);

            return result;
        }
    }
}
