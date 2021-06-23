﻿using AutoMapper;
using ECommerce.Api.Products.Db;
using ECommerce.Api.Products.Models;
using ECommerce.Api.Products.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace ECommerce.Api.Products.Providers
{
    public class ProductsProvider : IProductsProvider
    {
        private readonly ProductsDbContext dbContext;
        private readonly ILogger<ProductsProvider> logger;
        private readonly IMapper mapper;

        public ProductsProvider(ProductsDbContext dbContext,ILogger<ProductsProvider>logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;

            SeedData();
        }

        private void SeedData()
        {
            if (!dbContext.Products.Any())
            {
                dbContext.Products.Add(new Db.Product() { Id = 1, Name = "CPU", Price = 500, Inventory = 200 });
                dbContext.Products.Add(new Db.Product() { Id = 2, Name = "Monitor", Price = 400, Inventory = 150 });
                dbContext.Products.Add(new Db.Product() { Id = 3, Name = "Keyboard", Price = 300, Inventory = 300 });
                dbContext.Products.Add(new Db.Product() { Id = 4, Name = "Mouse", Price = 200, Inventory = 400 });
                dbContext.SaveChanges();
            }
                
           
        }

        public async Task<(bool isSuccess, IEnumerable<Models.Product> Products, string ErrorMessage)> GetProductsAsync()
        {           
            
            try
            {
                var products = await dbContext.Products.ToArrayAsync();
                if(products!=null && products.Any())
                {
                    var result = mapper.Map<IEnumerable<Db.Product>, IEnumerable<Models.Product>>(products);
                    
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

        public async Task<(bool isSuccess, Models.Product Product, string ErrorMessage)> GetProductAsync(int id)
        {

            try
            {
                var product = await dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
                if (product != null)
                {
                    var result = mapper.Map<Db.Product,Models.Product>(product);

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
