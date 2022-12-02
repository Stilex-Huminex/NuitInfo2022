using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuitInfo2022.Controllers.Shared;
using NuitInfo2022.Models.Entities;
using System.Diagnostics;

namespace NuitInfo2022.Controllers
{
    public class HomeController : RootController
    {
        public HomeController(ILogger<RootController> logger, ApplicationDbContext context) : base(logger, context)
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}