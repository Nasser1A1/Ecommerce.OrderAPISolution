using OrderApi.Application.DTO;
using OrderApi.Application.DTO.Converstions;
using OrderApi.Application.Interfaces;
using Polly;
using Polly.Registry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Application.Services
{
    public class OrderService(IOrder orderInterface,HttpClient httpClient,
        ResiliencePipelineProvider<string> resiliencePipeline) : IOrderService
    {
        public async Task<ProductDto> GetProduct(int productId)
        {
            // Call Product API Using httpClient
            // Redirect to api gateway
            var getProduct = await httpClient.GetAsync($"api/products/{productId}");
            if (!getProduct.IsSuccessStatusCode)
            {
                
                return null;
            }
           var product = await getProduct.Content.ReadFromJsonAsync<ProductDto>();
            
            return product!;
        }

        // Get User
        public async Task<AppUserDTO> GetUser(int userId)
        {
            // Redirect to api gateway
            var getUser = await httpClient.GetAsync($"api/Authentication/GetUser/{userId}");
            if (!getUser.IsSuccessStatusCode)
            {

                return null;
            }
            var user = await getUser.Content.ReadFromJsonAsync<AppUserDTO>();

            return user!;
        }

        // Get Product Details by ProductId
        public async Task<OrderDetailsDto> GetOrderDetails(int orderId)
        {
            // Call Product API Using httpClient
            // Redirect to api gateway
            // Prepare Order
            var order = await orderInterface.FindByIdAsync(orderId);
            if (order == null || order.Id <= 0)
            {
                return null;
            }
            // Get Retry Pipeline for resilience
            var retryPipeline = resiliencePipeline.GetPipeline("my-retry-pipeline");
            // prepare product
            var productDTO = await retryPipeline.ExecuteAsync(async token => await GetProduct(order.ProductId));
            // prepare user
            var appUserDTO = await retryPipeline.ExecuteAsync(async token => await GetUser(order.ClientId));

            if (appUserDTO == null)
            {
                appUserDTO = new AppUserDTO
                                     (
                                         Id: 1,// This should be replaced with actual user ID retrieval logic
                                         PhoneNumber: appUserDTO?.PhoneNumber ?? "Unknown Phone Number",
                                         Email: appUserDTO?.Email ?? "Unknown Email",
                                         Name: appUserDTO?.Name ?? "Unknown User",

                                         Address: appUserDTO?.Address ?? "Unknown Address",
                                         Password: "N/A", // Password should not be returned in DTOs, consider removing this field
                                         Role: "Client",
                                         CreatedAt: DateTime.UtcNow// Assuming role is Client, adjust as necessary
                                     );
            }
     
            // Populate OrderDetailsDto
            return new OrderDetailsDto
            (
                order.Id,
                productDTO.Id,
                appUserDTO.Id,
                appUserDTO.Name,
                appUserDTO.Email,
                appUserDTO.Address,
                productDTO.Name,
                order.PurchaseQuantity,
                appUserDTO.PhoneNumber,
                productDTO.Price * order.PurchaseQuantity,
                productDTO.Price,
                order.OrderDate


            );


        }

        // Get Orders by ClientId
        public async Task<IEnumerable<OrderDto>> GetOrdersByClientId(int clientId)
        {
            var orders = await orderInterface.GetOrdersAsync(o => o.ClientId == clientId);
            if (orders == null || !orders.Any())
            {
                return null;
            }
           var (_,_orders) = OrderConversion.FromEntity(null, orders);
            return _orders;
        }
      

     
    }
}
