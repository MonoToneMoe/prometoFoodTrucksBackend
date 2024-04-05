using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using prometoFoodTrucksBackend.Models;

// namespace prometoFoodTrucksBackend.Services.DbContext
{
    public class DataContext : DbContext
    {
        public DbSet<UserModel> UserInfo { get; set; }
        public DbSet<AppItemModel> UserInfo { get; set; }

        public DataContext(DbContextOptions options): base(options){}

        //this function will build out our table in the database
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}