using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace ReadmeBuilder;

internal static class StringExtensions
{
	internal static string FromKababToTitleCase(this string kebabCasedString)
	{
		StringBuilder result = new();
		string[] words = kebabCasedString.Split('-');
		foreach (string word in words)
		{
			result.Append(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(word));
			result.Append(' ');
		}
		if (result.Length > 0) result.Length--;
		return result.ToString();
	}

	internal static string ToSnakeCase(this string input)
	{
		// Replace all non-alphanumeric characters with underscores
		string snakeCase = Regex.Replace(input, @"[^a-zA-Z0-9]", "_");

		// Replace consecutive underscores with a single underscore
		snakeCase = Regex.Replace(snakeCase, @"_+", "_");

		// Convert to lower case
		snakeCase = snakeCase.ToLower();

		// Remove leading and trailing underscores
		snakeCase = snakeCase.Trim('_');

		return snakeCase;
	}

}