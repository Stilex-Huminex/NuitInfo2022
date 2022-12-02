using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuitInfo2022;
using NuitInfo2022.Controllers.Shared;
using NuitInfo2022.Models;
using NuitInfo2022.Models.Entities;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace NuitInfo2022.Controllers
{
    public class ApplicationUserController : RootController
    {
        public ApplicationUserController(ILogger<RootController> logger, ApplicationDbContext context) : base(logger, context)
        {
        }
        // GET: ApplicationUser
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("IsAdmin") != "True")
            {
                return NotFound();
            }
            return View(await _context.ApplicationUsers.ToListAsync());
        }

        // GET: ApplicationUser/Connexion
        public IActionResult Connexion()
        {
            return View();
        }
        // GET: ApplicationUser/Connexion
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Connexion(string email, string password)
        {
            ViewBag.Invalid_Email = null;
            ViewBag.Name = null;
            ViewBag.Email = null;
            ViewBag.Invalid_Password = null;
            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(m => m.Email == email);
            if(user == null)
            {
                @ViewBag.Invalid_Email = "Email incorrecte";
                return View("Connexion");
            }
            if(user.Password == HashPassword(password))
            {
                HttpContext.Session.SetString("UserId", user.Id.ToString());
                HttpContext.Session.SetString("IsAdmin", user.IsAdmin.ToString());
                ViewBag.Name = user.Name; ViewBag.Email = user.Email;
                return View("Connected");

            }
            else
            {
                @ViewBag.Invalid_Password = "Mot de passe Invalide";
                return View("Connexion");
            }
            
        }

        // GET: ApplicationUser/Connected
        public IActionResult Connected()
        {
            
            return View();
        }

        // GET: ApplicationUser/Connected
        public IActionResult Inscrit()
        {
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Inscription([Bind("Id,Name,Email,Password,IsAdmin")] ApplicationUser user )
        {
            ViewBag.Invalid_Email = null;
            var test = await _context.ApplicationUsers.FirstOrDefaultAsync(m => m.Email == user.Email);
            if(test != null)
            {
                ViewBag.Invalid_Email = user.Email + " est déja inscrit";
                return View("Inscription");
            }
            user.Password = HashPassword(user.Password);

            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                
                return View("Inscrit");
            }
           
            return View("Inscription");


        }
        // GET: ApplicationUser/Inscription
        public IActionResult Inscription()
        {
            return View();
        }



        // GET: ApplicationUser/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "True")
            {
                return NotFound();
            }

            if (id == null || _context.ApplicationUsers == null)
            {
                return NotFound();
            }

            var user = await _context.ApplicationUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: ApplicationUser/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("IsAdmin") != "True")
            {
                return NotFound();
            }
            return View();
        }


        // POST: ApplicationUser/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Password,IsAdmin")] ApplicationUser user)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "True")
            {
                return NotFound();
            }
            var test = await _context.ApplicationUsers.FirstOrDefaultAsync(m => m.Email == user.Email);
            if (test != null)
            {
                return View("Inscription");
            }
            user.Password = HashPassword(user.Password);

            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        public static string HashPassword(string password)
        {
            if (password == null) throw new ArgumentNullException("password");


            // Generate a 128-bit salt using a sequence of
            // cryptographically strong random bytes.
            byte[] salt = {0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1}; // divide by 8 to convert bits to bytes
            Console.WriteLine($"Salt: {Convert.ToBase64String(salt)}");

            // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password!,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
            return hashed;
        }

        // GET: ApplicationUser/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "True")
            {
                return NotFound();
            }
            if (id == null || _context.ApplicationUsers == null)
            {
                return NotFound();
            }

            var user = await _context.ApplicationUsers.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: ApplicationUser/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Email,Password,IsAdmin")] ApplicationUser user)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "True")
            {
                return NotFound();
            }
            if (id != user.Id)
            {
                return NotFound();
            }
            var test = await _context.ApplicationUsers.FirstOrDefaultAsync(m => m.Email == user.Email);
            
            if(user.Password == null)
            {
                user.Password = test.Password;
            }
            if(user.Name == null)
            {
                user.Password = test.Name;
            }
            if (user.Email == null)
            {
                user.Password = test.Email;
            }
            user.Password = HashPassword(user.Password);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: ApplicationUser/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "True")
            {
                return NotFound();
            }
            if (id == null || _context.ApplicationUsers == null)
            {
                return NotFound();
            }

            var user = await _context.ApplicationUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: ApplicationUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "True")
            {
                return NotFound();
            }
            if (_context.ApplicationUsers == null)
            {
                return Problem("Entity set 'ApplicationDbContext.User'  is null.");
            }
            var user = await _context.ApplicationUsers.FindAsync(id);
            if (user != null)
            {
                _context.ApplicationUsers.Remove(user);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(Guid id)
        {
          return _context.ApplicationUsers.Any(e => e.Id == id);
        }
    }
}
