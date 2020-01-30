using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FindFriends.Models;

namespace FindFriends.Context
{

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        public DbSet<Friend> friends { get; set; }
        public DbSet<CalculoHistoricoLog> CalculoHistoricoLog { get; set; }
        public DbSet<Friend> FriendInfo { get; set; }
        public DbSet<User> User { get; set; }
        
    }
}
