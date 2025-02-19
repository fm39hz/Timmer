namespace Timmer.Api.Domain.User;

using System.ComponentModel.DataAnnotations.Schema;
using Base;
using Constant;

[Table("users")]
public record User : BaseModel {
	[Column("role")] public Roles Role { get; init; } = Roles.None;
	[Column("name")] public required string Name { get; init; }
	[Column("email")] public required string Email { get; init; }
	[Column("emailConfirmed")] public bool EmailConfirmed { get; init; }
	[Column("password")] public string PasswordHash { get; init; } = null!;
}
