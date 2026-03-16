using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using vennAPI.Models;

namespace vennAPI.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base (options)
        {
            
        }

        public DbSet<UserModel> Users {get; set;}
        public DbSet<RoomModel> Rooms {get; set;}
        // public DbSet<> MyProperty { get; set; }
    }
}