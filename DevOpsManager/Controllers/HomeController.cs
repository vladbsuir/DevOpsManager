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

    // ����� ��� ��������� ���������� ������������ �� ������ ��� ����
    private AccessPermissions GetUserPermissions(string username)
    {
        var user = _context.Users.Include(u => u.Role)
                                 .FirstOrDefault(u => u.Username == username);

        if (user != null)
        {
            return user.Role?.Permissions ?? AccessPermissions.None;
        }

        return AccessPermissions.None; // ���� ������������ �� ������, ���������� ������ ����������
    }

    // ������� �������� � ���������� ������������
    public IActionResult Index()
    {
        var userPermissions = GetUserPermissions(User.Identity.Name); // �������� ���������� ������������
        ViewBag.UserPermissions = userPermissions; // �������� ���������� � �������������
        return View();
    }

    // �������� � ���������������� �����������
    public IActionResult Privacy()
    {
        var userPermissions = GetUserPermissions(User.Identity.Name); // �������� ���������� ������������
        ViewBag.UserPermissions = userPermissions; // �������� ���������� � �������������
        return View();
    }

    // �������� � ��������������
    public async Task<IActionResult> Users()
    {
        var userPermissions = GetUserPermissions(User.Identity.Name); // �������� ���������� ������������

        // �������� �� ���������� ��� ��������� �������������
        if ((userPermissions & AccessPermissions.Users) != AccessPermissions.Users)
        {
            return Forbid(); // ���� ��� ����, ��������� ������
        }

        ViewBag.UserPermissions = userPermissions; // �������� ���������� � �������������
        var users = await _context.Users.Include(u => u.Role).ToListAsync();
        return View(users);
    }

    public IActionResult Projects()
    {
        var userPermissions = GetUserPermissions(User.Identity.Name); // �������� ���������� ������������
        ViewBag.UserPermissions = userPermissions; // �������� ���������� � �������������
        return View();
    }

    public IActionResult Microservices()
    {
        var userPermissions = GetUserPermissions(User.Identity.Name); // �������� ���������� ������������
        ViewBag.UserPermissions = userPermissions; // �������� ���������� � �������������
        return View();
    }

    public IActionResult Deploy()
    {
        var userPermissions = GetUserPermissions(User.Identity.Name); // �������� ���������� ������������
        ViewBag.UserPermissions = userPermissions; // �������� ���������� � �������������
        return View();
    }
}
