using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuitInfo2022.Controllers.Shared;
using NuitInfo2022.Models;
using NuitInfo2022.Models.Entities;

namespace NuitInfo2022.Controllers
{
    public class UserMessageController : RootController
    {
        public UserMessageController(ILogger<RootController> logger, ApplicationDbContext context) : base(logger, context)
        {
        }

        // GET: UserMessageController
        public ActionResult Index()
        {
            return View(_context.UserMessages.Include(um => um.User).OrderByDescending(um => um.CreatedAt).ToArray());
        }

        // GET: UserMessageController/Create
        public ActionResult Create()
        {
            if (GetCurrentUser() == null) return RedirectToAction(nameof(Index));
            return View();
        }

        // POST: UserMessageController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string message)
        {
            if (message.Length > 500)
            {
                ViewBag["TooLong"] = "True";
                return View();
            }
            if (GetCurrentUser() == null) return RedirectToAction(nameof(Index));
            try
            {
                var newMessage = new UserMessageModel()
                {
                    Id = Guid.NewGuid(),
                    Message = message,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    UserId = GetCurrentUser().Id
                };
                _context.UserMessages.Add(newMessage);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // POST: UserMessageController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id)
        {
            try
            {
                var toRemove = _context.UserMessages.Where(um => um.Id == id).Single();
                if (toRemove.UserId == GetCurrentUser().Id)
                {
                    _context.UserMessages.Remove(toRemove);
                    _context.SaveChanges();
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
