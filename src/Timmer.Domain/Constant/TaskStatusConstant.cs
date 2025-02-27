namespace Timmer.Domain.Constant;

[Flags]
public enum TaskStatus {
	NotStarted = 0,
	InProgress = 1,
	Completed = 2
}
