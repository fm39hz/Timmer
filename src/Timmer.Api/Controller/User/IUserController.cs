namespace Timmer.Api.Controller.User;

using Application.Dto.User;
using Contract;
using Domain.Entity.User;

public interface IUserController : ICrudController<UserModel, IUserResponseDto, IUserRequestDto>;
