using System.Text;

namespace Tracing.Shared.Messages
{
    public static class HeaderParser
    {
        public static IEnumerable<string> GetHeaderValues(IDictionary<string, object> headers, string name)
        {
            if (headers.TryGetValue(name, out object value) && value is byte[] bytes)
            {
                return new[] { Encoding.UTF8.GetString(bytes) };
            }

            return Enumerable.Empty<string>();
        }
    }
}
