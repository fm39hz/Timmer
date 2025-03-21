namespace Timmer.Domain.Constant;

[Flags]
public enum Roles {
	None = 0,
	User = 1,
	Admin = 2
}

public static class RoleConstant {
	public const string NONE = "None";
	public const string ADMIN = "Admin";
	public const string USER = "User";
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
