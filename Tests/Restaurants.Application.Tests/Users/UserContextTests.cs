using Xunit;
using Restaurants.Application.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Restaurants.Domain.Constants;
using FluentAssertions;

namespace Restaurants.Application.Users.Tests
{
    public class UserContextTests
    {
        [Fact()]
        public void GetCurrentUser_WithAuthenticatedUser_ShouldReturnCurrentUser()
        {
            // Arrange

            var httpContextMock = new Mock<IHttpContextAccessor>();

            var claims = new List<Claim>()
            {
                new (ClaimTypes.NameIdentifier, "1"),
                new (ClaimTypes.Email, "test@test.com"),
                new (ClaimTypes.Role, UserRole.Admin),
                new (ClaimTypes.Role, UserRole.User),
                new ("Nationality", "German"),
                new ("DateOfBirth", new DateOnly(1990,1,1).ToString("yyyy-MM-dd"))

            };

            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "Test"));

            httpContextMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext()
            {
                User = user
            });

            var userContext = new UserContext(httpContextMock.Object);

            // Act
            var currentUser = userContext.GetCurrentUser();

            // Asset

            currentUser.Should().NotBeNull();
            currentUser.Id.Should().Be("1");
            currentUser.Email.Should().Be("test@test.com");
            currentUser.Roles.Should().ContainInOrder(UserRole.Admin,UserRole.User);
            currentUser.Nationality.Should().Be("German");
            currentUser.DateOfBirth.Should().Be(new DateOnly(1990, 1, 1));

        }

        [Fact]
        public void GetCurrentUser_WithUserContextNotPresent_ThrowsInvalidOperationException()
        {
            // Arrange

            var httpContextMock = new Mock<IHttpContextAccessor>();
            httpContextMock.Setup(x => x.HttpContext).Returns((HttpContext)null);


            var userContext = new UserContext(httpContextMock.Object);

            // Act

            Action action = () => userContext.GetCurrentUser();


            // Assert

            action.Should()
                .Throw<InvalidOperationException>()
                .WithMessage("User context is present");
        }

        
    }
}