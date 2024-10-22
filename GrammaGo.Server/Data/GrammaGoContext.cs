using Microsoft.EntityFrameworkCore;
using GrammaGo.Server.Models; 
using GrammaGo.Server.Data;  


namespace GrammaGo.Server.Data
{
    public class GrammaGoContext : DbContext
    {
        public GrammaGoContext(DbContextOptions<GrammaGoContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }    
    }
}
