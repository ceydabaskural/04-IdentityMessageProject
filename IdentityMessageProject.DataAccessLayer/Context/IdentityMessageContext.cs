using IdentityMessageProject.EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace IdentityMessageProject.DataAccessLayer.Context
{
    public class IdentityMessageContext : IdentityDbContext<AppUser, AppRole, int>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-F5CBDSU\\SQLEXPRESS;Database=IdentityMessageDb;Integrated Security=True;TrustServerCertificate=True;");
        }
        public DbSet<Message> Messages { get; set; }
    }
}
