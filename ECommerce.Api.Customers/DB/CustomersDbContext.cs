﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;

namespace ECommerce.Api.Customers.DB
{
    public class CustomersDbContext :   DbContext

    {
        public DbSet<Customer> Customers { get; set; }
        public CustomersDbContext(DbContextOptions options):base(options)
        {


        }
    }
}
