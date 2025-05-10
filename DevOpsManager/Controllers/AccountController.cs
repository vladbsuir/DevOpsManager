using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using DevOpsManager.Data;
using DevOpsManager.Models;
using System.Linq;
using System.Threading.Tasks;

namespace DevOpsManager.Controllers
{
    public class AccountController : Controller
    {
        private readonly DevOpsManagerContext _context;

        public AccountController(DevOpsManagerContext context)
        {
            _context = context;
        }


        // Метод для получения разрешений пользователя на основе его роли
        private AccessPermissions GetUserPermissions(string username)
        {
            var user = _context.Users.Include(u => u.Role)
                                     .FirstOrDefault(u => u.Username == username);

            if (user != null)
            {
                return user.Role?.Permissions ?? AccessPermissions.None;
            }

            return AccessPermissions.None; // Если пользователь не найден, возвращаем пустые разрешения
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                ModelState.AddModelError("", "Неверный логин или пароль.");
                var userPermissions = GetUserPermissions(User.Identity.Name); // Получаем разрешения пользователя
                ViewBag.UserPermissions = userPermissions; // Передаем разрешения в представление
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
            ViewBag.Roles = _context.Roles.ToList();
            ViewBag.UserPermissions = GetUserPermissions(User.Identity.Name); // Получаем разрешения пользователя
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(
            string username, string password, string email,
            string fullName, string slackHandle, string phoneNumber,
            string position, string department, string location,
            DateTime? birthDate)
        {
            if (_context.Users.Any(u => u.Username == username))
            {
                ModelState.AddModelError("", "Пользователь с таким именем уже существует.");
                ViewBag.Roles = _context.Roles.ToList();
                var userPermissions = GetUserPermissions(User.Identity.Name); // Получаем разрешения пользователя
                ViewBag.UserPermissions = userPermissions; // Передаем разрешения в представление
                return View();
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new User
            {
                Username = username,
                PasswordHash = passwordHash,
                Email = email,
                RoleId = 5, // Роль "Гость"
                CreatedAt = DateTime.UtcNow,
                FullName = fullName,
                SlackHandle = slackHandle,
                PhoneNumber = phoneNumber,
                Position = position,
                Department = department,
                Location = location,
                BirthDate = birthDate?.ToUniversalTime()
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            await SignInUser(user);
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        private async Task SignInUser(User user)
        {
            if (user.Role == null)
                user.Role = await _context.Roles.FindAsync(user.RoleId);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role?.Name ?? "User")
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProps = new AuthenticationProperties { IsPersistent = true };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), authProps);
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username == User.Identity.Name);

            if (user == null) return NotFound();
            var userPermissions = GetUserPermissions(User.Identity.Name); // Получаем разрешения пользователя
            ViewBag.UserPermissions = userPermissions; // Передаем разрешения в представление
            ViewBag.Roles = await _context.Roles.ToListAsync();
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(User updatedUser, string password)
        {
            var user = await _context.Users.FindAsync(updatedUser.Id);
            if (user == null) return NotFound();

            user.FullName = updatedUser.FullName;
            user.Email = updatedUser.Email;
            user.SlackHandle = updatedUser.SlackHandle;
            user.PhoneNumber = updatedUser.PhoneNumber;
            user.Position = updatedUser.Position;
            user.Department = updatedUser.Department;
            user.Location = updatedUser.Location;
            user.BirthDate = updatedUser.BirthDate?.ToUniversalTime();

            if (!string.IsNullOrWhiteSpace(password))
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Данные сохранены";
            return RedirectToAction("Profile");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (!User.IsInRole("Администратор"))
            {
                return Forbid();
            }
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Users", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == id);
            if (!User.IsInRole("Администратор"))
            {
                return Forbid();
            }
            if (user == null)
            {
                return NotFound();
            }

            var userPermissions = GetUserPermissions(User.Identity.Name); // Получаем разрешения пользователя
            ViewBag.UserPermissions = userPermissions; // Передаем разрешения в представление
            ViewBag.Roles = await _context.Roles.ToListAsync();  // Загружаем роли для выбора
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, string newPassword, int roleId)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Обновляем роль
            user.RoleId = roleId;

            // Если новый пароль не пустой, обновляем его
            if (!string.IsNullOrEmpty(newPassword))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            }

            _context.Update(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Users", "Home"); // Перенаправляем на страницу со списком пользователей
        }
    }
}
