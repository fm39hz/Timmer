namespace Timmer.Api.Constant;

[Flags]
public enum Roles {
	None = 0,
	User = 1,
	Admin = 2
}

public static class RolesExtensions {
	public static Roles ToRole(this string role) {
		try {
			return Enum.Parse<Roles>(role, true);
		}
		catch (Exception) {
			return Roles.None;
		}
	}
}
