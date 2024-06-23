using System.Net.Http.Headers;
using System.Reflection;

namespace cimbar.lib
{
    public static class ResourceHelper
    {
        public static string ReadResourceTxt(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var fr1 = assembly.GetManifestResourceNames().First(z => z.Contains(resourceName));

            using (Stream stream = assembly.GetManifestResourceStream(fr1))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public static byte[] ReadResourceBytes(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var arr = assembly.GetManifestResourceNames().ToArray();
            var fr1 = assembly.GetManifestResourceNames().First(z => z.ToLower().Contains(resourceName.ToLower()));

            MemoryStream ms = new MemoryStream();
            using (Stream stream = assembly.GetManifestResourceStream(fr1))
                stream.CopyTo(ms);

            ms.Seek(0, SeekOrigin.Begin);
            return ms.ToArray();
        }
    }
}