using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DevOpsManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using DevOpsManager.Data;

namespace DevOpsManager.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly DevOpsManagerContext _context;

    public HomeController(DevOpsManagerContext context)
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

    // Главная страница с доступными разрешениями
    public IActionResult Index()
    {
        var userPermissions = GetUserPermissions(User.Identity.Name); // Получаем разрешения пользователя
        ViewBag.UserPermissions = userPermissions; // Передаем разрешения в представление
        return View();
    }

    // Страница с конфиденциальной информацией
    public IActionResult Privacy()
    {
        var userPermissions = GetUserPermissions(User.Identity.Name); // Получаем разрешения пользователя
        ViewBag.UserPermissions = userPermissions; // Передаем разрешения в представление
        return View();
    }

    // Страница с пользователями
    public async Task<IActionResult> Users()
    {
        var userPermissions = GetUserPermissions(User.Identity.Name); // Получаем разрешения пользователя

        // Проверка на разрешения для просмотра пользователей
        if ((userPermissions & AccessPermissions.Users) != AccessPermissions.Users)
        {
            return Forbid(); // Если нет прав, запрещаем доступ
        }

        ViewBag.UserPermissions = userPermissions; // Передаем разрешения в представление
        var users = await _context.Users.Include(u => u.Role).ToListAsync();
        return View(users);
    }

    public IActionResult Projects()
    {
        var userPermissions = GetUserPermissions(User.Identity.Name); // Получаем разрешения пользователя
        ViewBag.UserPermissions = userPermissions; // Передаем разрешения в представление
        return View();
    }

    public IActionResult Microservices()
    {
        var userPermissions = GetUserPermissions(User.Identity.Name); // Получаем разрешения пользователя
        ViewBag.UserPermissions = userPermissions; // Передаем разрешения в представление
        return View();
    }

    public IActionResult Deploy()
    {
        var userPermissions = GetUserPermissions(User.Identity.Name); // Получаем разрешения пользователя
        ViewBag.UserPermissions = userPermissions; // Передаем разрешения в представление
        return View();
    }
}
