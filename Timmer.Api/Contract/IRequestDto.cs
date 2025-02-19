namespace Timmer.Api.Contract;

using Domain.Base;

public interface IRequestDto<out T> where T : BaseModel {
	public T ToModel();
}
