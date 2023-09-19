using System.Net.Mime;
using System.Text;

namespace POKA.Utils.Extensions
{
    public static class ObjectExtensions
    {
        public static StringContent ToStringContent(this object value)
            => new(
                    JsonConvert.SerializeObject(value),
                    Encoding.UTF8,
                    MediaTypeNames.Application.Json
                );
    }
}
