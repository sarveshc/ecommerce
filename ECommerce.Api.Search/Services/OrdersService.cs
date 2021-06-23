using ECommerce.Api.Search.Interfaces;
using ECommerce.Api.Search.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Api.Search.Services
{
    public class OrdersService : IOrderService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<OrdersService> logger;

        public OrdersService(IHttpClientFactory httpClientFactory, ILogger<OrdersService>logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
        }
        public async Task<(bool isSuccess, IEnumerable<Order> orders, string errorMessage)> GetOrderAsync(int customerId)
        {
            try
            {

            
            var client = httpClientFactory.CreateClient("OrdersService");
            var response = await client.GetAsync($"api/orders/{customerId}");
            if(response.IsSuccessStatusCode)
            {
                var content =await response.Content.ReadAsByteArrayAsync();
                var option = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                var result = JsonSerializer.Deserialize<IEnumerable<Order>>(content,option);

                return (true, result, null);
            }
            return (false, null, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
                
            }

        }

        private object JsonSerializerOptions()
        {
            throw new NotImplementedException();
        }
    }
}
