using FluentAssertions;
using Restaurants.Domain.Constants;
using Xunit;

namespace Restaurants.Application.Users.Tests;

public class CurrentUserTests
{

    //TestMethod_Scenario_ExpectResult

    [Theory()]
    [InlineData(UserRole.Admin)]
    [InlineData(UserRole.User)]
    public void IsInRole_WithMatchingRole_ShouldReturnTrue(string roleName)
    {
        // Arrange

        var currentUser = new CurrentUser("1", "test@test.com", [UserRole.Admin, UserRole.User],null,null); 


        // Act

        var isInRole = currentUser.IsInRole(roleName);


        // Assert

        isInRole.Should().BeTrue();
    }

    [Fact()]
    public void IsInRole_WithNoMatchingRole_ShouldReturnFalse()
    {
        // Arrange

        var currentUser = new CurrentUser("1", "test@test.com", [UserRole.Admin, UserRole.User], null, null);


        // Act

        var isInRole = currentUser.IsInRole(UserRole.Owner);


        // Assert

        isInRole.Should().BeFalse();
    }

    [Fact()]
    public void IsInRole_WithNoMatchingRoleCase_ShouldReturnFalse()
    {
        // Arrange

        var currentUser = new CurrentUser("1", "test@test.com", [UserRole.Admin, UserRole.User], null, null);


        // Act

        var isInRole = currentUser.IsInRole(UserRole.Admin.ToLower());


        // Assert

        isInRole.Should().BeFalse();
    }
}