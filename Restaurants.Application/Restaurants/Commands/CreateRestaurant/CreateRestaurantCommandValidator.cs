using FluentValidation;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandValidator : AbstractValidator<CreateRestaurantCommand>
{
    private readonly List<string> validCategories = ["Italian", "Japanese", "Sri Lankan", "American", "Indian"];
    public CreateRestaurantCommandValidator()
    {
        RuleFor(dto => dto.Name)
           .Length(3, 100)
           .WithMessage("Minimum length is 3 and max is 100 for the name");

        RuleFor(dto => dto.Description)
            .NotEmpty()
            .WithMessage("The description required");

        //RuleFor(dto => dto.Category)
        //    .Must(validCategories.Contains)
        //    .WithMessage("Invalid category!");

        RuleFor(dto => dto.Category)
            .Custom((value, context) =>
            {
                if (!validCategories.Contains(value))
                {
                    context.AddFailure(value, "Enter a valid category");
                }
            });

        RuleFor(dto => dto.ContactEmail)
            .EmailAddress()
            .WithMessage("Please provide a valid email address");

        RuleFor(dto => dto.PostCode)
            .Matches(@"^\d{2}-\d{3}$")
            .WithMessage("Valid post code reuired");
    }

}
