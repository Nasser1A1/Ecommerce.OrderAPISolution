using OrderApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Application.DTO.Converstions
{
    public static class OrderConversion
    {
        public static Order ToEntity(OrderDto order) => new ()
        {
            Id = order.Id,
            ProductId = order.ProductId,
            ClientId = order.ClientId,
            PurchaseQuantity = order.Quantity,
            OrderDate = order.OrderDate
        };

        public static (OrderDto?, IEnumerable<OrderDto>?) FromEntity(Order? order, IEnumerable<Order>? orders)
        {
            // return single order conversion
            if (order is not null)
            {
                var singleOrderDto = new OrderDto(
                    Id: order.Id,
                    ProductId: order.ProductId,
                    ClientId: order.ClientId,
                    Quantity: order.PurchaseQuantity,
                    OrderDate: order.OrderDate

                    );
                return (singleOrderDto, null);
            }
            // return multiple orders conversion
            if (orders is not null || order is null)
            {
                var orderDtos = orders.Select(o => new OrderDto(
                    Id: o.Id,
                    ProductId: o.ProductId,
                    ClientId: o.ClientId,
                    Quantity: o.PurchaseQuantity,
                    OrderDate: o.OrderDate
                ));
                return (null, orderDtos);
            }
            return (null, null);
        }
    }
}
