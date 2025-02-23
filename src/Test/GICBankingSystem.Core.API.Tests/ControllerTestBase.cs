using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GICBankingSystem.Core.API.Tests;

public abstract class ControllerTestBase<TController> : IDisposable where TController : Controller
{
    protected Mock<IMediator> _mediatorMock;
    protected TController _controller;
    private bool _disposed;

    [SetUp]
    public virtual void Setup()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = CreateController(_mediatorMock.Object);
    }

    protected abstract TController CreateController(IMediator mediator);

    [TearDown]
    public void TearDown()
    {
        Dispose(true);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _controller?.Dispose();
            }
            _disposed = true;
        }
    }

    protected static void AssertOkResult<T>(IActionResult result, Action<T> additionalAssertions = null)
    {
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        Assert.That(okResult.Value, Is.InstanceOf<T>());
        var response = (T)okResult.Value;
        additionalAssertions?.Invoke(response);
    }

    protected static void AssertBadRequest(IActionResult result, string expectedError)
    {
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        var badRequestResult = result as BadRequestObjectResult;
        Assert.That(badRequestResult.Value, Is.EqualTo(expectedError));
    }
}