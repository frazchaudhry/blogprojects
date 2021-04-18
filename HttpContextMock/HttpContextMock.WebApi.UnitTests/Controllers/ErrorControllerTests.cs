using System;
using FluentAssertions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace HttpContextMock.WebApi.Controllers
{
    public class ErrorControllerTests
    {
        [Fact]
        public void Error_WhenErrorInvoked_LogErrorToConsole()
        {
            // Arrange
            var logger = Substitute.For<ILogger<ErrorController>>();
            var controller = new ErrorController(logger)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
            var exceptionHandlerFeature = Substitute.For<IExceptionHandlerFeature>();
            var exception = Substitute.For<Exception>("There was an error.");
            exception.StackTrace.Returns("Sample Error stack trace.");
            exceptionHandlerFeature.Error.Returns(exception);
            controller.HttpContext.Features.Set(exceptionHandlerFeature);

            // Act
            var response = controller.Error();

            // Assert
            response.Should().NotBeNull();
            response.Should().BeAssignableTo<IActionResult>();
            response.Should().BeOfType<ObjectResult>();
            logger.Received(1).LogError($"{exception.Message} : {exception.StackTrace}");
        }
    }
}