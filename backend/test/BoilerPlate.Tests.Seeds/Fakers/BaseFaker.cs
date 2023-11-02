using Bogus;

namespace BoilerPlate.Tests.Seeds.Fakers;

public abstract class BaseFaker<TObject> : Faker<TObject>
    where TObject : class
{
    protected BaseFaker() : base("en") => Initialize();

    private void Initialize() => CustomInstantiator(Create);

    protected abstract TObject Create(Faker faker);
}