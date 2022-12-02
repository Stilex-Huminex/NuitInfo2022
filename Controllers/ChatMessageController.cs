using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuitInfo2022.Controllers.Shared;

namespace NuitInfo2022.Controllers
{
    public class ChatMessageController : RootController
    {
        public ChatMessageController(ILogger<RootController> logger, ApplicationDbContext context) : base(logger, context)
        {
        }

        [HttpPost]
        public IActionResult SendMessage(string message, Guid recipientId)
        {
            var sender = GetCurrentUser();
            var recipient = _context.ApplicationUsers.Where(u => u.Id == recipientId).Single();

            return Ok();
        }
    }
}
