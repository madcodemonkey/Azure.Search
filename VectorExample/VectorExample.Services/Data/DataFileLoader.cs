using Newtonsoft.Json.Linq;
using System.Reflection;

namespace VectorExample.Services;

internal class DataFileLoader
{
    public static Stream GetFileDataAsStream(string embeddedFileName)
    {
        // Based on: https://adamprescott.net/2012/07/26/files-as-embedded-resources-in-unit-tests/
        var asm = Assembly.GetExecutingAssembly();
        // Note: When assembly and namespace differ it appears that his is actually the namespace!
        var resource = string.Format("VectorExample.Services.Data.{0}", embeddedFileName);
        var stream = asm.GetManifestResourceStream(resource);
        return stream;
    }

    public static MemoryStream GetFileDataAsMemoryStream(string embeddedFileName)
    {
        using (var stream = GetFileDataAsStream(embeddedFileName))
        {
            var ms = new MemoryStream();
            stream.CopyTo(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }
    }

    public static byte[] GetFileDataAsByteArray(string embeddedFileName)
    {
        using (var ms = GetFileDataAsMemoryStream(embeddedFileName))
        {
            return ms.ToArray();
        }
    }

    public static string GetFileDataAsString(string embeddedFileName)
    {
        string result;

        using (var ms = GetFileDataAsMemoryStream(embeddedFileName))
        using (var sr = new StreamReader(ms))
        {
            result = sr.ReadToEnd();
        }

        return result;
    }

    public static JObject GetFileDataAsJObject(string embeddedFileName)
    {
        string data = GetFileDataAsString(embeddedFileName);
        return JObject.Parse(data);
    }

    public static JArray GetFileDataAsJArray(string embeddedFileName)
    {
        string data = GetFileDataAsString(embeddedFileName);
        return JArray.Parse(data);
    }
}
