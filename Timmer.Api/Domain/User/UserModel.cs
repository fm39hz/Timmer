namespace Timmer.Api.Domain.User;

using Base;
using UserTask;
using Constant;
using System.ComponentModel.DataAnnotations.Schema;

[Table("users")]
public sealed record UserModel : BaseModel {
	public UserModel(UserModel user) : base(user) {
		Id = user.Id;
		Role = user.Role;
		Name = user.Name;
		Email = user.Email;
		IsVerified = user.IsVerified;
		PasswordHash = user.PasswordHash;
		Tasks = user.Tasks;
	}

	[Column("role")] public Roles Role { get; init; } = Roles.None;
	[Column("name")] public string Name { get; init; } = string.Empty;
	[Column("email")] public string Email { get; init; } = string.Empty;
	[Column("is_verified")] public bool IsVerified { get; init; }
	[Column("password")] public string PasswordHash { get; init; } = string.Empty;
	[InverseProperty("user")] public ICollection<UserTaskModel> Tasks { get; init; } = [];
}
