using System.Reflection;

namespace Plugin.Maui.MarkdownView.Test.Services;

static class FileReader
{
    public static string ReadEmbeddedResource(string filepath)
    {
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Plugin.Maui.MarkdownView.Test.Resources." + filepath);
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}