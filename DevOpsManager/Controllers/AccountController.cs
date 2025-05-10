using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using DevOpsManager.Data;
using DevOpsManager.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DevOpsManager.Controllers
{
    public class AccountController : Controller
    {
        private readonly DevOpsManagerContext _context;

        public AccountController(DevOpsManagerContext context)
        {
            _context = context;
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login() => View();

        // POST: /Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _context.Users
                .Include(u => u.Role) // загружаем роль пользователя
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                ModelState.AddModelError("", "Неверный логин или пароль.");
                return View();
            }

            user.LastLoginAt = DateTime.UtcNow;
            _context.Update(user);
            await _context.SaveChangesAsync();

            await SignInUser(user);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.Roles = _context.Roles.ToList(); // Передаём список ролей в представление
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string username, string password, string email, int roleId)
        {
            if (_context.Users.Any(u => u.Username == username))
            {
                ModelState.AddModelError("", "Пользователь с таким именем уже существует.");
                ViewBag.Roles = _context.Roles.ToList(); // повторная передача ролей в случае ошибки
                return View();
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new User
            {
                Username = username,
                PasswordHash = passwordHash,
                Email = email,
                RoleId = roleId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Авторизация
            var role = await _context.Roles.FindAsync(roleId);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, role?.Name ?? "User")
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties { IsPersistent = true };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity),
                authProperties
            );

            return RedirectToAction("Index", "Home");
        }



        // POST: /Account/Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        // Приватный метод для аутентификации пользователя
        private async Task SignInUser(User user)
        {
            // Подгрузить роль, если не подгружена
            if (user.Role == null)
            {
                user.Role = await _context.Roles.FindAsync(user.RoleId);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role?.Name ?? "User")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties
            );
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username == User.Identity.Name);

            if (user == null) return NotFound();

            var roles = await _context.Roles.ToListAsync();
            ViewBag.Roles = roles;

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(int id, string name, string password, int roleId)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            user.Username = name;

            if (!string.IsNullOrWhiteSpace(password))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password); // предполагается использование хеширования
            }

            //user.RoleId = roleId;

            await _context.SaveChangesAsync();

            return RedirectToAction("Profile");
        }
    }

}

