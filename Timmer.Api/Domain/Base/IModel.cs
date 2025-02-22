namespace Timmer.Api.Domain.Base;

public interface IModel {
	/// <summary>
	///     Entity Id
	/// </summary>
	public Guid Id { get; init; }
}
