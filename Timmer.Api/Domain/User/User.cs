namespace Timmer.Api.Domain.User;

using System.ComponentModel.DataAnnotations.Schema;
using Base;
using Constant;

[Table("users")]
public sealed record User : BaseModel {
	public User(User user) : base(user) {
		Id = user.Id;
		Role = user.Role;
		Name = user.Name;
		Email = user.Email;
		IsVerified = user.IsVerified;
		PasswordHash = user.PasswordHash;
	}

	[Column("role")] public Roles Role { get; init; } = Roles.None;
	[Column("name")] public string Name { get; init; } = string.Empty;
	[Column("email")] public string Email { get; init; } = string.Empty;
	[Column("is_verified")] public bool IsVerified { get; init; }
	[Column("password")] public string PasswordHash { get; init; } = string.Empty;
}
