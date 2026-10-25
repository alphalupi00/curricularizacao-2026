using Microsoft.AspNetCore.Mvc;

namespace DULCIESBOLOS.Controllers
{
    public class GaleriaBolosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}