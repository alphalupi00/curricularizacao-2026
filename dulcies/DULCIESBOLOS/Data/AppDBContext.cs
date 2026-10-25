using Microsoft.EntityFrameworkCore;
using DULCIESBOLOS.Models;

namespace DULCIESBOLOS.Data{
     public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

        public DbSet<Funcionario> Funcionarios { get; set; } 
        public DbSet<Cargo> Cargos { get; set; } 
         public DbSet<DULCIESBOLOS.Models.Cliente> Cliente { get; set; }
         public DbSet<DULCIESBOLOS.Models.Bolo> Bolo { get; set; }
         public DbSet<DULCIESBOLOS.Models.Acrescimo> Acrescimo { get; set; }
         public DbSet<DULCIESBOLOS.Models.Pagamento> Pagamento { get; set; }
         public DbSet<DULCIESBOLOS.Models.Venda> Venda { get; set; }
       
       
    }
}
