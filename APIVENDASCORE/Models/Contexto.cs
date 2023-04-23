using Microsoft.EntityFrameworkCore;

namespace APIVENDASCORE.Models


{
    public class Contexto : DbContext
    {
        public Contexto()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=Vendas;User ID=vendas;Password=vendas;TrustServerCertificate=true;");
        }
        public Contexto(DbContextOptions<Contexto> options)
       : base(options)
        {
        }

        public DbSet<Usuario> Usuario { get; set; } = null!;
    }
}
