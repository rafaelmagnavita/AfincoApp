using AfincoApp.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace AfincoApp.DAL
{
    public class AfincoContext : DbContext
    {

        public AfincoContext() : base("AConnectionString")
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Balanco> Balancos { get; set; }
        public DbSet<Movimentacao> Movimentacoes { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}