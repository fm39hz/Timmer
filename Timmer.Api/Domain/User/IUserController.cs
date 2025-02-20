namespace Timmer.Api.Domain.User;

using Contract;
using Dto;

public interface IUserController : ICrudController<User, UserResponseDto, UserRequestDto>;
