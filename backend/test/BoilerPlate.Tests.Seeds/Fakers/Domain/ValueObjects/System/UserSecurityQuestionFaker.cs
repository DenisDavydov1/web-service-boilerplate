using Bogus;
using BoilerPlate.Core.Utils;
using BoilerPlate.Data.Domain.ValueObjects.System;

namespace BoilerPlate.Tests.Seeds.Fakers.Domain.ValueObjects.System;

public class UserSecurityQuestionFaker : BaseFaker<UserSecurityQuestion>
{
    public string Question { get; set; }
    public string Answer { get; set; }

    public UserSecurityQuestionFaker()
    {
        Question = FakerHub.Random.String(30);
        Answer = FakerHub.Random.String(10);
    }

    protected override UserSecurityQuestion Create(Faker faker)
    {
        var valueObject = new UserSecurityQuestion
        {
            Question = Question,
            AnswerHash = HashingUtils.HashBCrypt(Answer)
        };

        return valueObject;
    }
}