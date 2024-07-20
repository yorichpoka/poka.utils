using Newtonsoft.Json.Serialization;

namespace POKA.Utils.Infrastructure.SqlServer
{
    internal class Constants
    {
        public static readonly JsonSerializerSettings DefaultJsonSerializerSettings = new()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Formatting = Formatting.None,
            MaxDepth = 3
        };
    }
}
