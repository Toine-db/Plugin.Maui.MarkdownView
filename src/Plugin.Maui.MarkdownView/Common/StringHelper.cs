using System.Text.RegularExpressions;

namespace Plugin.Maui.MarkdownView.Common;

public static class StringHelper
{
    public static bool HasHttp(this string path)
    {
        return Uri.TryCreate(path, UriKind.Absolute, out var uriResult)
            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }

    public static bool TryCreateUri(this string path, out Uri? uri)
    {
        return Uri.TryCreate(path, UriKind.Absolute, out uri)
            && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
    }

    public static string RemoveSpecialCharactersExcept(this string input, params char[] allowedSpecialChars)
    {
        var escapedAllowed = string.Concat(allowedSpecialChars.Select(c => Regex.Escape(c.ToString())));
        var pattern = $"[^A-Za-z0-9{escapedAllowed}]";
        return Regex.Replace(input, pattern, "");
    }
}