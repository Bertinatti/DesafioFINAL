using DesafioFINAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DesafioFINAL.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Produto> Produto { get; set; }
        public DbSet<LogProdutos> LogProdutos { get; set; }
        public DbSet<Usuario> Usuario { get; set; }

    }
}