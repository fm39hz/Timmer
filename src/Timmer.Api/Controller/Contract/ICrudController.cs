namespace Timmer.Api.Controller.Contract;

using Application.Dto;
using Application.Dto.Common;
using Domain.Entity;

public interface IReadOneController<TModel, TResponse>
	where TModel : IModel
	where TResponse : IResponseDto {
	/// <summary>
	///     Find one entity with id in database
	/// </summary>
	/// <param name="id">Id of model</param>
	/// <returns></returns>
	public Task<IValueHttpResult<TResponse>> FindOne(Guid id);
}

public interface IReadAllController<TModel, TResponse>
	where TModel : IModel
	where TResponse : IResponseDto {
	/// <summary>
	///     Find all entity in database
	/// </summary>
	/// <returns></returns>
	public Task<IValueHttpResult<IEnumerable<TResponse>>> FindAll();
}

public interface ICreateController<TModel, TResponse, in TRequest>
	where TModel : IModel
	where TResponse : IResponseDto
	where TRequest : IRequestDto<TModel> {
	/// <summary>
	///     Create new entity in database
	/// </summary>
	/// <param name="entity">the entity value</param>
	/// <returns></returns>
	public Task<IValueHttpResult<TResponse>> Create(TRequest entity);
}

public interface IUpdateController<TModel, TResponse, in TRequest>
	where TModel : IModel
	where TResponse : IResponseDto
	where TRequest : IRequestDto<TModel> {
	/// <summary>
	///     Delete one entity that has id
	/// </summary>
	/// <param name="id">Id of model</param>
	/// <returns></returns>
	public Task<IValueHttpResult<TResponse>> Update(Guid id, TRequest entity);
}

public interface IDeleteController<TModel> where TModel : IModel {
	public Task<IValueHttpResult<int>> Delete(Guid id);
}

/// <summary>
///     An interface that declare Crud actions controller
/// </summary>
/// <typeparam name="TModel">Target model of controller</typeparam>
/// <typeparam name="TResponse">Response dto</typeparam>
/// <typeparam name="TRequest">Request dto</typeparam>
public interface ICrudController<TModel, TResponse, in TRequest> :
	IReadOneController<TModel, TResponse>,
	IReadAllController<TModel, TResponse>,
	ICreateController<TModel, TResponse, TRequest>,
	IUpdateController<TModel, TResponse, TRequest>,
	IDeleteController<TModel>
	where TModel : IModel
	where TResponse : IResponseDto
	where TRequest : IRequestDto<TModel> {
}
