namespace Timmer.Api.Domain.User;

using Bogus;
using Constant;

public static class UserDataGenerator {
	public static UserModel Generate(bool isAdmin = false, string prehashPassword = "") => RuleSet
			.RuleFor(user => user.Role, faker => isAdmin ? Roles.Admin : Roles.None)
			.RuleFor(user => user.PasswordHash, faker => prehashPassword != "" ? prehashPassword : faker.Random.Word())
			.Generate();

	public static Faker<UserModel> RuleSet => new Faker<UserModel>()
			.RuleFor(static user => user.Name, static faker => faker.Person.FullName)
			.RuleFor(static user => user.Email, static faker => faker.Person.Email);
}
