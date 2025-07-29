
using System.ComponentModel.DataAnnotations;

namespace OrderApi.Application.DTO
{
    public record AppUserDTO(
        int Id,
        [Required]
        string PhoneNumber,
        [Required, EmailAddress]
        string Email,
        [Required]
        string Name,
        [Required]
        string Address,
        [Required]
        string Password,
        [Required]
        string Role,
        DateTime CreatedAt
    );

}
