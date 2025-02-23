namespace GICBankingSystem.Core.Application.Tests.Base;

public abstract class ValidatorTestBase<TValidator> where TValidator : class
{
    protected TValidator Validator;

    [SetUp]
    public virtual void Setup()
    {
        Validator = CreateValidator();
    }

    protected abstract TValidator CreateValidator();
}
