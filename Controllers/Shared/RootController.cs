using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuitInfo2022.Models.Entities;

namespace NuitInfo2022.Controllers.Shared
{
    public class RootController : Controller
    {
        protected readonly ILogger<RootController> _logger;
        protected readonly ApplicationDbContext _context;

        public RootController(ILogger<RootController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        protected ApplicationUser GetCurrentUser()
        {
            var userIdString = HttpContext.Session.GetString("UserId");

            if (userIdString == null) return null;

            var userId = Guid.Parse(userIdString);
            return _context.ApplicationUsers.Where(u => u.Id == userId).Single();
        }
    }
}
