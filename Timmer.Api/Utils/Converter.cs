namespace Timmer.Api.Utils;
public static class Converter {
	public static string ToSnakeCase(string input) => string.Concat(input.Select(static (x, i) => i > 0 && char.IsUpper(x) ? "_" + char.ToLower(x) : x.ToString())).ToLower(System.Globalization.CultureInfo.CurrentCulture);
}
