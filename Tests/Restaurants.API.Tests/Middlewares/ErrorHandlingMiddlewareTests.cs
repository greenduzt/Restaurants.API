using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Xunit;


namespace Restaurants.API.Middlewares.Tests
{
    public class ErrorHandlingMiddlewareTests
    {
        [Fact()]
        public async Task InvokeAsync_WhenNoExceptionThrown_ShouldCallNextDelegate()
        {
            // Arrange

            var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
            var errorHandlingMiddleware = new ErrorHandlingMiddleware(loggerMock.Object);
            var httpContext = new DefaultHttpContext();
            var nextDelegateMock = new Mock<RequestDelegate>();

            // Act

            await errorHandlingMiddleware.InvokeAsync(httpContext, nextDelegateMock.Object);

            // Assert

            nextDelegateMock.Verify(next => next.Invoke(httpContext), Times.Once);
        }

        [Fact()]
        public async Task InvokeAsync_WhenNotFoundExceptionThrown_ShouldSetStatusCode404()
        {
            // Arrange

            var notFountException = new NotFoundException(nameof(Restaurant),"1");
            var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
            var httpContext = new DefaultHttpContext();
            var errorHandlingMiddleware = new ErrorHandlingMiddleware(loggerMock.Object);
            var nextDelegateMock = new Mock<RequestDelegate>();

            // Act

            await errorHandlingMiddleware.InvokeAsync(httpContext, _ => throw notFountException);

            // Assert
            httpContext.Response.StatusCode.Should().Be(404);

        }

        [Fact()]
        public async Task InvokeAsync_WhenForbidExceptionThrown_ShouldSetStatusCode403()
        {
            // Arrange

            var forbidException = new ForbidException();
            var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
            var httpContext = new DefaultHttpContext();
            var errorHandlingMiddleware = new ErrorHandlingMiddleware(loggerMock.Object);
            var nextDelegateMock = new Mock<RequestDelegate>();

            // Act

            await errorHandlingMiddleware.InvokeAsync(httpContext, _ => throw forbidException);

            // Assert
            httpContext.Response.StatusCode.Should().Be(403);

        }

        [Fact()]
        public async Task InvokeAsync_WhenGenericExceptionThrown_ShouldSetStatusCode500()
        {
            // Arrange

            var exception = new Exception();
            var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
            var httpContext = new DefaultHttpContext();
            var errorHandlingMiddleware = new ErrorHandlingMiddleware(loggerMock.Object);
            var nextDelegateMock = new Mock<RequestDelegate>();

            // Act

            await errorHandlingMiddleware.InvokeAsync(httpContext, _ => throw exception);

            // Assert
            httpContext.Response.StatusCode.Should().Be(500);

        }
    }
 }