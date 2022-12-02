using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuitInfo2022.Controllers.Shared;
using NuitInfo2022.Models.Entities;

namespace NuitInfo2022.Controllers
{
    public class GameController : RootController
    {
        public GameController(ILogger<RootController> logger, ApplicationDbContext context) : base(logger, context)
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
