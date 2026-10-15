namespace DulciesBolos.Domain.Entities;

public class Venda
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ClienteId { get; set; }
    public Cliente? Cliente { get; set; }
    public DateTime DataVenda { get; set; } = DateTime.UtcNow;
    public decimal ValorTotal { get; set; }
    public List<ItemVenda> Itens { get; set; } = new();
}

public class ItemVenda
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid VendaId { get; set; }
    public Guid BoloId { get; set; }
    public Bolo? Bolo { get; set; }
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
}