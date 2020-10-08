using Layout.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Layout.Db
{
    public class Database : DbContext
    {
        protected IConfiguration configuration;
        public Database(DbContextOptions<Database> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder model)
        {

        }

        public DbSet<Session> Sessions { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Product> Selected { get; set; } 
    }
}
