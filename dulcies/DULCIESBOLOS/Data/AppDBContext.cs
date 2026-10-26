using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using DULCIESBOLOS.Models;

namespace DULCIESBOLOS.Data{
     public class AppDBContext : IdentityDbContext<Usuario>
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

        public DbSet<Funcionario> Funcionarios { get; set; } 
         public DbSet<DULCIESBOLOS.Models.Bolo> Bolo { get; set; }
         public DbSet<DULCIESBOLOS.Models.Acrescimo> Acrescimo { get; set; }
         public DbSet<DULCIESBOLOS.Models.Pagamento> Pagamento { get; set; }
         public DbSet<DULCIESBOLOS.Models.Venda> Venda { get; set; }
       
       
    }
}
