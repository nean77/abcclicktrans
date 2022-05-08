using abcclicktrans.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace abcclicktrans.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<TransportAddress> TransportAddress { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<TransportOrder> TransportOrders { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
    }
}