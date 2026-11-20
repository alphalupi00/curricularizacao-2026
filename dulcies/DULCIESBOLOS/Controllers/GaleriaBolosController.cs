using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization; //modificações
using Microsoft.AspNetCore.Mvc;

namespace DULCIESBOLOS.Controllers
{
    [AllowAnonymous] //modificações
    public class GaleriaBolosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}