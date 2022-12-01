using Microsoft.AspNetCore.Mvc;

namespace NuitInfo2022.Controllers
{
    public class GameController : Controller
    {
        private readonly ILogger<GameController> _logger;
        private ApplicationDbContext _context;

        public GameController(ILogger<GameController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
