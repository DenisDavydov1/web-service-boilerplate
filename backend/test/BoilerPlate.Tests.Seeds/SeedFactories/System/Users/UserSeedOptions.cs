using BoilerPlate.Tests.Seeds.Fakers.Domain.Entities.System;
using BoilerPlate.Tests.Seeds.Fakers.Domain.ValueObjects.System;

namespace BoilerPlate.Tests.Seeds.SeedFactories.System.Users;

public class UserSeedOptions
{
    public Action<UserFaker>? RuleFor { get; set; }

    public Action<UserSecurityQuestionFaker>? SecurityQuestionRuleFor { get; set; }
}