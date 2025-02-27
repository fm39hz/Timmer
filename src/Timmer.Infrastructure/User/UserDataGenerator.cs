namespace Timmer.Infrastructure.User;

using Bogus;
using Common.Constant;
using Domain.User;

public static class UserDataGenerator {
	private static Faker<UserModel> RuleSet => new Faker<UserModel>()
		.RuleFor(static user => user.Name, static faker => faker.Person.FullName)
		.RuleFor(static user => user.Email, static faker => faker.Person.Email);

	public static UserModel Generate(bool isAdmin = false, string rawPassword = "") => RuleSet
		.RuleFor(user => user.Role, _ => isAdmin? Roles.Admin : Roles.None)
		.RuleFor(user => user.PasswordHash, faker => rawPassword != ""? rawPassword : faker.Random.Word())
		.Generate();
}
