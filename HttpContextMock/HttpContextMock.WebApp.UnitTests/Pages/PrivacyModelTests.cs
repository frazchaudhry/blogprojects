using System.Security.Claims;
using System.Security.Principal;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace HttpContextMock.WebApp.Pages
{
    public class PrivacyModelTests
    {
        private readonly ILogger<PrivacyModel> _logger;

        public PrivacyModelTests()
        {
            _logger = Substitute.For<ILogger<PrivacyModel>>();
        }

        [Fact]
        public void UserName_WhenInvokedAndValidContext_ReturnsUserName()
        {
            // Arrange
            var privacyModel = new PrivacyModel(_logger)
            {
                PageContext = new PageContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
            var user = Substitute.For<ClaimsPrincipal>();
            privacyModel.PageContext.HttpContext.User = user;
            var identity = Substitute.For<IIdentity>();
            privacyModel.HttpContext.User.Identity.Returns(identity);
            const string expectedUserName = "Joe Jim";
            privacyModel.HttpContext.User.Identity?.Name.Returns(expectedUserName);

            // Act
            var actual = privacyModel.UserName();

            // Assert
            actual.Should().NotBeNullOrEmpty();
            actual.Should().BeOfType<string>();
            actual.Should().Be(expectedUserName);
        }
    }
}