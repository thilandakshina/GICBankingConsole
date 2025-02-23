using Moq;

namespace GICBankingSystem.Core.Application.Tests.Base;

public abstract class HandlerTestBase<THandler, TService> where THandler : class where TService : class
{
    protected Mock<TService> ServiceMock;
    protected THandler Handler;

    [SetUp]
    public virtual void Setup()
    {
        ServiceMock = new Mock<TService>();
        Handler = CreateHandler();
    }

    protected abstract THandler CreateHandler();
}