namespace Timmer.Application.Domain.User;

using Contract;
using Dto;

public interface IUserController : ICrudController<UserModel, UserResponseDto, UserRequestDto>;
