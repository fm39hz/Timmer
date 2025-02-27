namespace Timmer.Application.Dto;

using Domain.Entity;

public interface IRequestDto<out T> where T : IModel {
	public T ToModel();
}
