namespace DulciesBolos.Domain.Entities;

public class Acrescimo
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Nome { get; set; } = string.Empty;
    public decimal Preco { get; set; }
}