using Microsoft.AspNetCore.Mvc;

namespace NuitInfo2022.Controllers
{
    public class InfoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Sida()
        {
            return View();
        }
        public IActionResult Hepatite()
        {
            return View();
        }
        public IActionResult Papillomavirus()
        {
            return View();
        }
        public IActionResult Chlamydia()
        {
            return View();
        }

        // -- Et bien d'autres
    }
}
