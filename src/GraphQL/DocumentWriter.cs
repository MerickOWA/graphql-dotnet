using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace GraphQL
{
    public interface IDocumentWriter
    {
        Task WriteAsync<T>(Stream stream, T value);
    }

    public static class DocumentWriterExtensions
    {
        internal static readonly Encoding Utf8Encoding = new UTF8Encoding(false);

        /// <summary>
        /// Writes the <paramref name="value"/> to string.
        /// </summary>
        public static async Task<string> WriteToStringAsync<T>(this IDocumentWriter writer, T value)
        {
            using (var stream = new MemoryStream())
            {
                await writer.WriteAsync(stream, value).ConfigureAwait(false);
                stream.Position = 0;
                using (var reader = new StreamReader(stream, Utf8Encoding))
                {
                    return await reader.ReadToEndAsync().ConfigureAwait(false);
                }
            }
        }
    }
}