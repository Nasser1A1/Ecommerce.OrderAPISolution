using eCommerce.SharedLib.ResponseT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Application.DTO;
using OrderApi.Application.DTO.Converstions;
using OrderApi.Application.Interfaces;
using OrderApi.Application.Services;

namespace OrderApi.Presentation.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController(IOrder orderInterface,IOrderService orderService) : ControllerBase
    {
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
        {
            var ordersList = await orderInterface.GetAllAsync();
            if (ordersList == null || !ordersList.Any())
            {
                return NotFound($"No orders found");
            }
            var (_,list) = OrderConversion.FromEntity(null,ordersList);
       
            return !list.Any() ? NotFound("No orders found") : Ok(list);
        }
        

        [HttpGet("debug-auth")]
        public IActionResult Debug()
        {
            return Ok(new
            {
                IsAuthenticated = User.Identity?.IsAuthenticated,
                Name = User.Identity?.Name,
                Claims = User.Claims.Select(c => new { c.Type, c.Value })
            });
        }

        [HttpGet("client/{clientId:int}")]
        public async Task<ActionResult<Response>> GetClinetOrders(int clientId)
        {
            if(clientId <=0 )
            {
                return BadRequest("Invalid Client Id");
            }
            var ordersList = await orderService.GetOrdersByClientId(clientId);
            if (ordersList == null || !ordersList.Any())
            {
                return new Response(false, $"No orders found for client with ID {clientId}");
            }
            
            return !ordersList.Any() ? NotFound("No orders found for this Client") : Ok(ordersList);
        }

        [HttpGet("details/{orderId:int}")]
        public async Task<ActionResult<OrderDetailsDto>> GetOrderDetails(int orderId)
        {
            if (orderId <= 0)
            {
                return BadRequest("Invalid Order Id");
            }
            var ordersDetails = await orderService.GetOrderDetails(orderId);
     

            return ordersDetails.OrderId >= 0 ? Ok(ordersDetails) : NotFound("No Order Found");
        }



        [HttpGet("{orderId:int}")]
        public async Task<ActionResult<OrderDto>> GetOrder(int orderId)
        {
            var order = await orderInterface.FindByIdAsync(orderId);
            if (order == null)
            {
                return NotFound($"No orders found with this id {orderId}");
            }
            var (_order, _) = OrderConversion.FromEntity(order, null);

            return _order != null ? Ok(_order) : NotFound("No orders found with this id");
        }


        [HttpPost("create")]
        public async Task<ActionResult<Response>> CreateOrder(OrderDto orderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var getEntity = OrderConversion.ToEntity(orderDto);
            var response = await orderInterface.CreateAsync(getEntity);
            return response.Flag ? Ok(response) : BadRequest(response);
        }

        [HttpPut("update")]
        public async Task<ActionResult<Response>> UpdateOrder(OrderDto orderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var getEntity = OrderConversion.ToEntity(orderDto);
            var response = await orderInterface.UpdateAsync(getEntity);
            return response.Flag ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("delete")]
        public async Task<ActionResult<Response>> DeleteOrder(OrderDto orderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var getEntity = OrderConversion.ToEntity(orderDto);
            var response = await orderInterface.DeleteAsync(getEntity);
            return response.Flag ? Ok(response) : BadRequest(response);
        }

      
    }
}
