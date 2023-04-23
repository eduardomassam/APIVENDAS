using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace APICORE.Models
{
    public class Contexto : DbContext
    {
        public Contexto(DbContextOptions<Contexto> options)
        : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; } = null!;
    }
}
