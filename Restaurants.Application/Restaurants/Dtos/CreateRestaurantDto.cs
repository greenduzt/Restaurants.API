using System.ComponentModel.DataAnnotations;

namespace Restaurants.Application.Restaurants.Dtos;

public class CreateRestaurantDto
{
    [StringLength(100,MinimumLength =3,ErrorMessage ="Length should be greater than 3 and less than 100 characters")]
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;

    [Required(ErrorMessage ="Enter a valid category")]
    public string Category { get; set; } = default!;
    public bool HasDelivery { get; set; }

    [EmailAddress(ErrorMessage ="Enter a valid email address")]
    public string? ContactEmail { get; set; }

    [Phone(ErrorMessage ="Please provide a valid phone number")]
    public string? ContactNumber { get; set; }
    public string? City { get; set; }
    public string? Street { get; set; }
    public string? PostCode { get; set; }
}
