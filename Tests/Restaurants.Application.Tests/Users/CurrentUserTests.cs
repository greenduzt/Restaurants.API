using FluentAssertions;
using Restaurants.Domain.Constants;
using Xunit;

namespace Restaurants.Application.Users.Tests;

public class CurrentUserTests
{
    // TestMethod_Scenario_ExpectResult
    [Theory()]
    [InlineData(UserRole.Admin)]
    [InlineData(UserRole.User)]
    public void IsInRole_WithMatchingRole_ShouldReturnTrue(string roleName)
    {
        // arrange
        var currentUser = new CurrentUser("1", "test@test.com", [UserRole.Admin, UserRole.User], null, null);

        // act

        var isInRole = currentUser.IsInRole(roleName);

        // assert

        isInRole.Should().BeTrue();

    }


    [Fact()]
    public void IsInRole_WithNoMatchingRole_ShouldReturnFalse()
    {
        // arrange
        var currentUser = new CurrentUser("1", "test@test.com", [UserRole.Admin, UserRole.User], null, null);

        // act

        var isInRole = currentUser.IsInRole(UserRole.Owner);

        // assert

        isInRole.Should().BeFalse();

    }

    [Fact()]
    public void IsInRole_WithNoMatchingRoleCase_ShouldReturnFalse()
    {
        // arrange
        var currentUser = new CurrentUser("1", "test@test.com", [UserRole.Admin, UserRole.User], null, null);

        // act

        var isInRole = currentUser.IsInRole(UserRole.Admin.ToLower());

        // assert

        isInRole.Should().BeFalse();

    }
}