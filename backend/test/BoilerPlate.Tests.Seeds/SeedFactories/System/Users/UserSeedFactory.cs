using BoilerPlate.Data.DAL.UnitOfWork;
using BoilerPlate.Data.Domain.Entities.System;
using BoilerPlate.Tests.Seeds.Fakers.Domain.Entities.System;
using BoilerPlate.Tests.Seeds.Fakers.Domain.ValueObjects.System;

namespace BoilerPlate.Tests.Seeds.SeedFactories.System.Users;

public static class UserSeedFactory
{
    public static async Task<User> SeedAsync(IUnitOfWork unitOfWork, Action<UserSeedOptions>? configure = default)
    {
        var options = new UserSeedOptions();
        configure?.Invoke(options);

        var securityQuestionFaker = new UserSecurityQuestionFaker();
        options.SecurityQuestionRuleFor?.Invoke(securityQuestionFaker);
        var securityQuestions = new[] { securityQuestionFaker.Generate() };

        var userFaker = new UserFaker(securityQuestions);
        options.RuleFor?.Invoke(userFaker);
        var user = userFaker.Generate();

        await unitOfWork.Repository<User>().AddAsync(user);
        await unitOfWork.SaveAsync();

        return user;
    }
}