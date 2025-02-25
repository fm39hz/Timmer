namespace Timmer.Application.Utils;

using System.Globalization;

public static class Converter {
	public static string ToSnakeCase(string input) => string
		.Concat(input.Select(static (x, i) => i > 0 && char.IsUpper(x)? "_" + char.ToLower(x) : x.ToString()))
		.ToLower(CultureInfo.CurrentCulture);
}
