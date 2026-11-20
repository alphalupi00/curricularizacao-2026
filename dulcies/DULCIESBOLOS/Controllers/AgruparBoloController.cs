using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization; //modificações
using Microsoft.AspNetCore.Mvc;
using DULCIESBOLOS.Services;

namespace DULCIESBOLOS.Controllers
{
    [Authorize(Roles = "Admin")] //modificações
    public class AgruparBoloController : Controller
    {
        private readonly IPivotService _pivotService;

        public AgruparBoloController(IPivotService pivotService)
        {
            _pivotService = pivotService;
        }

        // GET: /AgruparBolo/Pivot
        public async Task<IActionResult> Pivot()
        {
            var viewModel = await _pivotService.ObterPivotVendasPorSaborAsync();
            return View(viewModel);
        }

        // GET: /AgruparBolo
        public async Task<IActionResult> Index()
        {
            return await Pivot();
        }
    }
}
