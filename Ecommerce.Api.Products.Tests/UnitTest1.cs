using AutoMapper;
using ECommerce.Api.Products.Db;
using ECommerce.Api.Products.Profiles;
using ECommerce.Api.Products.Providers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Ecommerce.Api.Products.Tests
{
    public class ProductsServiceTest
    {
        [Fact]
        public async Task GetProductsReturnsAllProducts()
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(nameof(GetProductsReturnsAllProducts))
                .Options;
            var dbContext = new ProductsDbContext(options);

            CreatePrduct(dbContext);

            var productProfile = new  ProductProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(productProfile));
            var mapper = new Mapper(configuration);
            var productProvider = new ProductsProvider(dbContext, null, mapper);
            var product = await productProvider.GetProductsAsync();
            Assert.True(product.isSuccess);
            Assert.True(product.Products.Any());
            Assert.Null(product.ErrorMessage);

        }

        private void CreatePrduct(ProductsDbContext dbContext)
        {
            for(int i = 1; i <= 10; i++)
            {
                dbContext.Products.Add(new Product()
                {
                    Id = i,
                    Name = Guid.NewGuid().ToString(),
                    Inventory = i + 100,
                    Price = (decimal)(i * 0)

                });
                
            }
            dbContext.SaveChanges();

        }
    }
}
