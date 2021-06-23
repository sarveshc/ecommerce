using ECommerce.Api.Search.Interfaces;
using ECommerce.Api.Search.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Search.Services
{
    public class SearchServices : ISearchService
    {
        private readonly IOrderService orderService;
        private readonly IProductsService productsService;
        private readonly ICustomersService customersService;

        public SearchServices(IOrderService orderService, IProductsService productsService, ICustomersService customersService)
        {
            this.orderService = orderService;
            this.productsService = productsService;
            this.customersService = customersService;
        }
        public async Task<(bool isSuccess, dynamic searchResult)> SearchAsync(int customerId)
        {
            var customerResult = await customersService.GetCustomerAsync(customerId);
            var orderResult = await orderService.GetOrderAsync(customerId);
            var productResult = await productsService.GetProductsAsync();
            if(orderResult.isSuccess)
             {
                foreach (var order in orderResult.orders)
                {
                    foreach (var item in order.Items)
                    {
                        item.ProductName = productResult.IsSuccess? productResult.Products.FirstOrDefault(p => p.Id == item.ProductId)?.Name:
                            "Product infromation is not available";

                    }
                }
                var result = new
                {
                    Customer =customerResult.IsSucess?customerResult.Customer:new {Name="Custmer Name is not available!"},
                    Orders = orderResult.orders
                };
                return (true, result);

            }
            return (false, null);

        }
    }
}
