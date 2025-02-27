namespace Timmer.Common.Contract;

using Model;

public interface IRequestDto<out T> where T : IModel {
	public T ToModel();
}
