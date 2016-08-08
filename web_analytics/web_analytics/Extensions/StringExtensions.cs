namespace web_analytics.Extensions
{
	public static class StringExtensions
	{
		public static bool IsNotNullOrEmpty(this string s)
		{
			return !string.IsNullOrWhiteSpace(s);
		}

		public static bool IsNullOrEmpty(this string s)
		{
			return string.IsNullOrWhiteSpace(s);
		}
	}
}