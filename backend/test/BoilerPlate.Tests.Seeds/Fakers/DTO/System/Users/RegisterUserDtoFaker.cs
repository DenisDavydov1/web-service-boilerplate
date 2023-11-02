using Bogus;
using BoilerPlate.Core.Constants;
using BoilerPlate.Data.DTO.System.Users.Requests;

namespace BoilerPlate.Tests.Seeds.Fakers.DTO.System.Users;

public class RegisterUserDtoFaker : BaseFaker<RegisterUserDto>
{
    public string Login { get; set; }

    public string Password { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string LanguageCode { get; set; }

    public IDictionary<string, string> SecurityQuestions { get; set; }

    public RegisterUserDtoFaker()
    {
        Login = FakerHub.Random.String2(10);
        Password = FakerHub.Internet.Password(prefix: "1Aa");
        LanguageCode = LanguageCodes.English;
        SecurityQuestions = FakerHub
            .Make(3, _ => (FakerHub.Random.String(30), FakerHub.Random.String(10)))
            .ToDictionary(k => k.Item1, v => v.Item2);
    }

    protected override RegisterUserDto Create(Faker faker)
    {
        var dto = new RegisterUserDto
        {
            Login = Login,
            Password = Password,
            Name = Name,
            Email = Email,
            LanguageCode = LanguageCode,
            SecurityQuestions = SecurityQuestions
        };

        return dto;
    }
}