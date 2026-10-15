using DulciesBolos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DulciesBolos.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options) { }

    public DbSet<Funcionario> Funcionarios => Set<Funcionario>();
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Bolo> Bolos => Set<Bolo>();
    public DbSet<Acrescimo> Acrescimos => Set<Acrescimo>();
    public DbSet<Venda> Vendas => Set<Venda>();
    public DbSet<ItemVenda> ItensVenda => Set<ItemVenda>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Bolo>().Property(b => b.Preco).HasPrecision(18, 2);
        modelBuilder.Entity<Acrescimo>().Property(a => a.Preco).HasPrecision(18, 2);
        modelBuilder.Entity<Venda>().Property(v => v.ValorTotal).HasPrecision(18, 2);
        modelBuilder.Entity<ItemVenda>().Property(i => i.PrecoUnitario).HasPrecision(18, 2);
    }
}