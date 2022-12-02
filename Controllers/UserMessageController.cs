using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
            return View(_context.UserMessages.ToArray());
        }

        // GET: UserMessageController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserMessageController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string message)
        {
            if (GetCurrentUser() == null) return Forbid();
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

        // GET: UserMessageController/Edit/5
        public ActionResult Edit(Guid id)
        {
            if (GetCurrentUser() == null) RedirectToAction(nameof(Index));
            try
            {
                var userId = HttpContext.Session.GetString("UserId");
                var message = _context.UserMessages.Where(u => u.Id == id && u.UserId == GetCurrentUser().Id || GetCurrentUser().IsAdmin);
                return View(message);
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // POST: UserMessageController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserMessageController/Delete/5
        public ActionResult Delete(Guid id)
        {
            return View();
        }

        // POST: UserMessageController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
