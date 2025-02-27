namespace Timmer.Domain.User;

using Common.Contract;

public interface IUserController : ICrudController<UserModel, IUserResponseDto, IUserRequestDto>;
