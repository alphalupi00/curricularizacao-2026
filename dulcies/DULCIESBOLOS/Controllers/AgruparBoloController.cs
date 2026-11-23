using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DULCIESBOLOS.Services;

namespace DULCIESBOLOS.Controllers
{
    [Authorize(Roles = "Admin")]
    //injeçao de dependencia - PivotService
    public class AgruparBoloController : Controller
    {
        private readonly PivotService _pivotService;

        public AgruparBoloController(PivotService pivotService)
        {
            _pivotService = pivotService;
        }
    //acaba aqui

        // GET: /AgruparBolo/Pivot
        public async Task<IActionResult> Pivot()
        {
            var viewModel = await _pivotService.ObterPivotVendasPorSaborAsync();
            return View(viewModel);
        }
    }


    
}
