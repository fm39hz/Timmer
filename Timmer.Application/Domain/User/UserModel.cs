namespace Timmer.Application.Domain.User;

using System.ComponentModel.DataAnnotations.Schema;
using Base;
using Constant;
using UserTask;

public sealed record UserModel : BaseModel {
	public UserModel(UserModel user) : base(user) {
		Role = user.Role;
		Name = user.Name;
		Email = user.Email;
		IsVerified = user.IsVerified;
		PasswordHash = user.PasswordHash;
		Tasks = user.Tasks;
	}

	public Roles Role { get; init; } = Roles.None;
	public string Name { get; init; } = string.Empty;
	public string Email { get; init; } = string.Empty;
	public bool IsVerified { get; init; }
	public string PasswordHash { get; init; } = string.Empty;

	[InverseProperty("user")] public ICollection<UserTaskModel> Tasks { get; init; } = [];
}
