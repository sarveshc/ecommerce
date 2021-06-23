using AutoMapper;
using ECommerce.Api.Customers.DB;

using ECommerce.Api.Customers.Interfaces;
using ECommerce.Api.Customers.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Customers.Providers
{
    public class CustomersProvider : ICustomersProvider
    {
        private readonly CustomersDbContext dbContext;
        private readonly ILogger<CustomersProvider> logger;
        private readonly IMapper mapper;

        public CustomersProvider(CustomersDbContext dbContext,ILogger<CustomersProvider>logger,IMapper mapper )
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;

            SeedData();
        }

        private void SeedData()
        {
           if(!dbContext.Customers.Any())
            {
                dbContext.Customers.Add(new DB.Customer() { Id = 1, Name = "Johan Smith", Address = "NY" });
                dbContext.Customers.Add(new DB.Customer() { Id = 2, Name = "Peter Nova", Address = "WDC" });
                dbContext.Customers.Add(new DB.Customer() { Id = 3, Name = "Michel", Address = "AT" });
                dbContext.SaveChanges();
            }
        }

        public async Task<(bool isSuccess, IEnumerable<Models.Customer> Customers, string ErrorMessage)> GetCustomersSync()
        {
            try
            {
                var customers = await dbContext.Customers.ToArrayAsync();
                if(customers!=null && customers.Any())
                {
                    var result = mapper.Map<IEnumerable<DB.Customer>, IEnumerable<Models.Customer>>(customers);
                    return (true, result, null);

                }
                return (false, null, "Not Found");

            }
            catch (Exception ex)
            {

                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool isSuccess, Models.Customer Customers, string ErrorMessage)> GetCustomerSync(int id)
        {
            try
            {
                var customer = await dbContext.Customers.FirstOrDefaultAsync(c => c.Id == id);
                if (customer != null)
                {
                    var result = mapper.Map<DB.Customer, Models.Customer>(customer);

                    return (true, result, null);

                }
                return (false, null, "Not Found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
