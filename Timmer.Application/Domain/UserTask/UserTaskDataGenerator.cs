namespace Timmer.Application.Domain.UserTask;

using Bogus;
using Constant;
using User;

public static class UserTaskDataGenerator {
	private static Faker<UserTaskModel> RuleSet => new Faker<UserTaskModel>()
		.RuleFor(static task => task.Name, static faker => faker.Rant.Random.Words())
		.RuleFor(static task => task.Description, static faker => faker.Rant.Review())
		.RuleFor(static task => task.StartTime, static faker => faker.Date.Past())
		.RuleFor(static task => task.EndTime, static faker => faker.Date.Future())
		.RuleFor(static task => task.Status, static faker => faker.Random.Enum<TaskStatus>());

	public static UserTaskModel Generate(UserModel model) =>
		RuleSet.RuleFor(static task => task.User, _ => model).Generate();
}
