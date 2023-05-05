using Microsoft.EntityFrameworkCore;

namespace APIVENDASCORE.Models


{
    public class Contexto : DbContext
    {
        public Contexto()
        {
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=Vendas;User ID=sa;Password=sa;TrustServerCertificate=true;",
        //sqlServerOptionsAction: sqlOptions =>
        //{
        //    sqlOptions.EnableRetryOnFailure(
        //        maxRetryCount: 5, // número máximo de tentativas de conexão
        //        maxRetryDelay: TimeSpan.FromSeconds(30), // tempo máximo de espera entre tentativas
        //        errorNumbersToAdd: null
        //    );
        //});
        //}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=localhost,22;Initial Catalog=Vendas;User ID=sa;Password=Docker158*@!;TrustServerCertificate=true;",
                sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5, // número máximo de tentativas de conexão
                        maxRetryDelay: TimeSpan.FromSeconds(30), // tempo máximo de espera entre tentativas
                        errorNumbersToAdd: null
                    );
                });
        }

        // Para o NroSeq ser incremental
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<HistPedido>()
            //        .Property(hp => hp.CodPed)
            //        .ValueGeneratedNever();

            modelBuilder.Entity<HistPedido>()
                .Property(hp => hp.NroSeq)
                .ValueGeneratedOnAdd();

       

        }

        public Contexto(DbContextOptions<Contexto> options)
       : base(options)
        {
        }

        public DbSet<Usuario> Usuario { get; set; } = null!;
        public DbSet<AvaliacaoPedidos> AvaliacaoPedidos { get; set; } = null!;
        public DbSet<HistPedido> HistPedido { get; set; } = null!;
        public DbSet<Pedidos> Pedidos { get; set; } = null!;
        public DbSet<Status> Status { get; set; } = null!;
        public DbSet<StatusPedido> StatusPedido { get; set; } = null!;
        public DbSet<Transportadora> Transportadora { get; set; } = null!;
        public DbSet<Vendedor> Vendedor { get; set; } = null!;

    }
}
