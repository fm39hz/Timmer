namespace Timmer.Application.Contract;

using Domain.Base;

public interface IRequestDto<out T> where T : IModel {
	public T ToModel();
}
