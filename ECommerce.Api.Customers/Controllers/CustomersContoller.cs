using ECommerce.Api.Customers.Interfaces;
using ECommerce.Api.Customers.Providers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Customers.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public class CustomersContoller : ControllerBase
    {
        private readonly ICustomersProvider customersProvider;

        public CustomersContoller(ICustomersProvider customersProvider)
        {
            this.customersProvider = customersProvider;
        }
        [HttpGet]
        public async Task<ActionResult> GetCustomersAsync()
        {
            var result = await customersProvider.GetCustomersSync();
            if(result.isSuccess)
            {
                return Ok(result.Customers);
            }
            return NotFound();

        }
        [HttpGet("{id}")]        
        public async Task<ActionResult> GetCustomersAsync(int id)
        {
            var result = await customersProvider.GetCustomerSync(id);
            if (result.isSuccess)
            {
                return Ok(result.Customers);
            }
            return NotFound();

        }
    }
}
