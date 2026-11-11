using System.Threading.Tasks;
using DULCIESBOLOS.Models.ViewModel;

namespace DULCIESBOLOS.Services
{
    public interface IPivotService
    {
        Task<PivotBolosViewModel> ObterPivotVendasPorSaborAsync(string tipoAgrupamento = "cliente");
    }
}
