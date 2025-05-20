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
    public async Task<IActionResult> Index()
    {
        // �������� ���������� ������� � �������� ��������
        var totalUsers = await _context.Users.CountAsync();
        var totalProjects = await _context.Projects.CountAsync();
        var totalMicroservices = await _context.Microservices.CountAsync();
        var totalDeployments = await _context.Deployments.CountAsync();

        // ������� ���������� � ViewBag
        ViewBag.TotalUsers = totalUsers;
        ViewBag.TotalProjects = totalProjects;
        ViewBag.TotalMicroservices = totalMicroservices;
        ViewBag.TotalDeployments = totalDeployments;

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

    public async Task<IActionResult> Projects()
    {
        ViewBag.UserPermissions = GetUserPermissions(User.Identity.Name);
        // ��������� ���� �������� � ����������
        var projects = await _context.Projects
            .Include(p => p.Owner)
            .OrderBy(p => p.Name)
            .ToListAsync();
        return View(projects);
    }

    // ��������������� �� ���������� �������������
    public IActionResult Microservices()
    {
        return RedirectToAction("Index", "Microservice"); // �������������� �� ���������� Microservice
    }

    public IActionResult Deploy()
    {
        return RedirectToAction("Index", "Deploy"); // �������������� �� ���������� Deploy
    }
}
