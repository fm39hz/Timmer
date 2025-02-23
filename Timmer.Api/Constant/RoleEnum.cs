namespace Timmer.Api.Constant;

[Flags]
public enum Roles {
	None = 0,
	User = 1,
	Admin = 2
}

public static class RoleValues {
	public const string None = "None";
	public const string Admin = "Admin";
	public const string User = "User";
}

public static class RolesFactory {
	public static Roles ToRole(this string role) {
		try {
			Enum.TryParse(role, true, out Roles result);
			return result;
		}
		catch (Exception) {
			return Roles.None;
		}
	}
}
