using AutoMapper;
using GICBankingSystem.Core.Application.Interfaces.Repository;
using Moq;

namespace GICBankingSystem.Core.Application.Tests.Base;

public abstract class ServiceTestBase<TService> where TService : class
{
    protected Mock<IUnitOfWork> UnitOfWorkMock;
    protected Mock<IMapper> MapperMock;
    protected TService Service;

    [SetUp]
    public virtual void Setup()
    {
        UnitOfWorkMock = new Mock<IUnitOfWork>();
        MapperMock = new Mock<IMapper>();
        Service = CreateService();
    }

    protected abstract TService CreateService();
}