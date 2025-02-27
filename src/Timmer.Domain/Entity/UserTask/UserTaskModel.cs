namespace Timmer.Domain.Entity.UserTask;

using System.ComponentModel.DataAnnotations.Schema;
using User;

public sealed record UserTaskModel : BaseModel {
	public UserTaskModel(UserTaskModel task) : base(task) {
		Name = task.Name;
		Description = task.Description;
		User = task.User;
		Status = task.Status;
		StartTime = task.StartTime;
		EndTime = task.EndTime;
	}

	public string Name { get; init; } = string.Empty;
	public string Description { get; init; } = string.Empty;
	public DateTimeOffset StartTime { get; init; }
	public DateTimeOffset EndTime { get; init; }
	public TaskStatus Status { get; init; }

	[ForeignKey("user_id")] public UserModel User { get; init; } = null!;
}
