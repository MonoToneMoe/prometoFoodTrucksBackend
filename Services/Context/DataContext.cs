using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using prometoFoodTrucksBackend.Models;

namespace prometoFoodTrucksBackend.Services.Context
{
    public class DataContext
    {
        public DbSet<UserModel> UserInfo { get; set; }
        public DbSet<AppItemModel> BlogInfo { get; set; }

        public DataContext(DbContextOptions options): base(options){}

        //this function will build out our table in the database
        protected override void OnModelCreating(AppItemModel builder)
        {
            base.OnModelCreating(builder);
        }
    }
}