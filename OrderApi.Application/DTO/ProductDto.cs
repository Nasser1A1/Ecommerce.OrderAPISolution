using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Application.DTO
{
    public record ProductDto(
        int Id,
        [Required]
        string Name,
        [Required,DataType(DataType.Currency) 
        ,Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        decimal Price,
        [Required, Range(0, int.MaxValue)]
        int StockQuantity
    );

}
