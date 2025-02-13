namespace Plugin.Maui.MarkdownView.Common;

public class MarkdownParseExceptionEventArgs(Exception parseException) : EventArgs
{
	public Exception ParseException { get; set; } = parseException;
}