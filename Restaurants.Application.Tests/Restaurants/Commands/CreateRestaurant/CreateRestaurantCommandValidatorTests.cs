using FluentValidation.TestHelper;
using Xunit;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant.Tests;

public class CreateRestaurantCommandValidatorTests
{
    [Fact()]
    public void Validator_ForValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange

        var command = new CreateRestaurantCommand()
        {
            Name = "Test",
            Description = "Test Desc",
            Category = "Italian",
            ContactEmail = "test@test.com",
            PostCode = "12-345"
        };

        var validator = new CreateRestaurantCommandValidator();

        // Act

        var result =  validator.TestValidate(command); 


        // Assert

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact()]
    public void Validator_ForInvalidCommand_ShouldHaveValidationErrors()
    {
        // Arrange

        var command = new CreateRestaurantCommand()
        {
            Name = "Te",
            Description = "Test Desc",
            Category = "Ita",
            ContactEmail = "@test.com",
            PostCode = "12345"
        };

        var validator = new CreateRestaurantCommandValidator();

        // Act

        var result = validator.TestValidate(command);


        // Assert

        result.ShouldHaveValidationErrorFor(c => c.Name);
        //result.ShouldHaveValidationErrorFor(c => c.Category);
        result.ShouldHaveValidationErrorFor(c => c.ContactEmail);
        result.ShouldHaveValidationErrorFor(c => c.PostCode);
    }

    [Theory()]
    [InlineData("Italian")]
    [InlineData("Maxican")]
    [InlineData("Japanese")]
    [InlineData("American")]
    [InlineData("Indian")]
    public void Validator_ForValidCategory_ShouldNotHaveValidationErrorsForCategoryProperty(string category)
    {

        // Arrange

        var validator = new CreateRestaurantCommandValidator();
        var command = new CreateRestaurantCommand() { Category = category };

        // Act

        var result = validator.TestValidate(command);

        // Assert

        result.ShouldNotHaveValidationErrorFor(c => c.Category);

    }

    [Theory()]
    [InlineData("10220")]
    [InlineData("102-20")]
    [InlineData("10 220")]
    [InlineData("10-2 20")]
    

    public void Validator_ForInvalidPostalCode_ShouldHaveValidationErrorsForPostalCodeProperty(string postCode)
    {

        // Arrange 

        var validator = new CreateRestaurantCommandValidator();
        var command = new CreateRestaurantCommand() { PostCode = postCode };

        // Act

        var result = validator.TestValidate(command);

        // Assert

        result.ShouldHaveValidationErrorFor(c => c.PostCode);

    }
}