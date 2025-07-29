using System.ComponentModel.DataAnnotations;
namespace OrderApi.Application.DTO
{
    public record OrderDetailsDto(
       [Required] int OrderId,
       [Required] int ProductId,
       [Required] int ClientId,
       [Required] string ClientName,
       [Required,EmailAddress] string Email,
       [Required] string Address,
       [Required] string ProductName,
       [Required] int Quantity,
       [Required] string PhoneNumber,
       [Required,DataType(DataType.Currency)] decimal TotalPrice,
       [Required,DataType(DataType.Currency)] decimal UnitPrice,
       [Required] DateTime OrderDate
    );

}
