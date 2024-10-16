using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PROG_P1.Models;

namespace PROG_P1.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<PROG_P1.Models.Claims> Claims { get; set; } = default!;
        public DbSet<PROG_P1.Models.Document> Documents { get; set; } = default!;
    }
}
