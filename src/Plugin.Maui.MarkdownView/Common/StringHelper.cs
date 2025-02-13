using System.Text;

namespace Plugin.Maui.MarkdownView.Common;

public static class StringHelper
{
	public static bool HasHttp(this string path)
	{
		return path.StartsWith("http:")
		       || path.StartsWith("https:");
	}

	public static string RemoveSpecialCharactersExcept(this string input, char[] allowedSpecialChars)
	{
		var sb = new StringBuilder();
		foreach (var chr in input)
		{
			if (chr is >= '0' and <= '9'
			    || chr is >= 'A' and <= 'Z'
			    || chr is >= 'a' and <= 'z'
			    || allowedSpecialChars.Contains(chr))
			{
				sb.Append(chr);
			}
		}

		return sb.ToString();
	}
}