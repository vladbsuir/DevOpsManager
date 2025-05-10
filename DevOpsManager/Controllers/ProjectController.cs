using DevOpsManager.Data;
using DevOpsManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DevOpsManager.Controllers
{
    public class ProjectController : Controller
    {
        private readonly DevOpsManagerContext _context;

        public ProjectController(DevOpsManagerContext context)
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

        // GET: Project
        public async Task<IActionResult> Index()
        {
            var projects = await _context.Projects
                .Include(p => p.Owner)
                .ToListAsync();
            ViewBag.UserPermissions = GetUserPermissions(User.Identity.Name); // Получаем разрешения пользователя
            return View(projects);
        }

        // GET: Project/Edit/{id}
        public async Task<IActionResult> Edit(int? id)
        {
            Project project;

            // Если id не передано или равно нулю, создаем новый проект
            if (id == null || id == 0)
            {
                project = new Project
                {
                    StartDate = DateTime.UtcNow,
                    Status = "Активен"
                };
            }
            else
            {
                project = await _context.Projects
                    .Include(p => p.Owner)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (project == null)
                    return NotFound();
            }

            // Подготовка списка пользователей для выбора владельца проекта
            ViewBag.Users = new SelectList(_context.Users, "Id", "FullName", project.OwnerId);
            ViewBag.UserPermissions = GetUserPermissions(User.Identity.Name); // Получаем разрешения пользователя
            return View(project);
        }

        // POST: Project/Save
        [HttpPost]
        public async Task<IActionResult> Save(Project project)
        {
            // Преобразуем даты в формат UTC перед сохранением
            project.StartDate = project.StartDate.ToUniversalTime();
            project.EndDate = project.EndDate?.ToUniversalTime();

            /*if (!ModelState.IsValid)
            {
                // Повторно передаем список пользователей на форму при ошибке валидации
                ViewBag.Users = new SelectList(_context.Users, "Id", "FullName", project.OwnerId);
                return View("Edit", project);
            */

            // Если у проекта новый Id (т.е. проект не существует), добавляем его в БД
            if (project.Id == 0)
                _context.Projects.Add(project);
            else
                _context.Projects.Update(project);

            await _context.SaveChangesAsync();
            return RedirectToAction("Projects", "Home");
        }

        // GET: Project/Delete/{id}
        public async Task<IActionResult> Delete(int id)
        {
            var project = await _context.Projects
                .Include(p => p.Owner)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Project/DeleteConfirmed/{id}
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Projects", "Home");
        }
    }
}
