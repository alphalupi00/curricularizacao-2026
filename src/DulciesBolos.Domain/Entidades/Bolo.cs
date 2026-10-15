namespace DulciesBolos.Domain.Entities;

public class Bolo
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Sabor { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public int QuantidadeEstoque { get; set; }
}