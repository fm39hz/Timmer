namespace Timmer.Api.Domain.UserTask;

using Base;
using User;
using Constant;
using System.ComponentModel.DataAnnotations.Schema;

[Table("tasks")]
public sealed record UserTaskModel : BaseModel {
	public UserTaskModel(UserTaskModel task) : base(task) {
		Id = task.Id;
		Name = task.Name;
		Description = task.Description;
		User = task.User;
		Status = task.Status;
		StartTime = task.StartTime;
		EndTime = task.EndTime;
	}

	[Column("user")][ForeignKey("user_id")] public UserModel User { get; init; } = null!;
	[Column("name")] public string Name { get; init; } = string.Empty;
	[Column("description")] public string Description { get; init; } = string.Empty;
	[Column("start_time")] public DateTimeOffset StartTime { get; init; }
	[Column("end_time")] public DateTimeOffset EndTime { get; init; }
	[Column("status")] public TaskStatus Status { get; init; }
}
