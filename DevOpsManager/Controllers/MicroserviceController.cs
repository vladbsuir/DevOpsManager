using DevOpsManager.Data;
using DevOpsManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DevOpsManager.Controllers
{
    public class MicroserviceController : Controller
    {
        private readonly DevOpsManagerContext _context;

        public MicroserviceController(DevOpsManagerContext context)
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


        // Метод для получения данных для редактирования сервиса
        public async Task<IActionResult> Edit(int? id)
        {
            Microservice microservice;

            // Если id не передано или равно нулю, создаем новый проект
            if (id == null || id == 0)
            {
                microservice = new Microservice
                {
                };
            }
            else
            {
                microservice = await _context.Microservices
                        .Include(m => m.Project) // Включаем проект для связи
                        .FirstOrDefaultAsync(m => m.Id == id);


                if (microservice == null)
                    return NotFound();
            }
            // Получаем сервис по ID


            
            // Передаем данные для отображения в представлении
            ViewBag.Projects = new SelectList(await _context.Projects.ToListAsync(), "Id", "Name");
            ViewBag.UserPermissions = GetUserPermissions(User.Identity.Name); // Получаем разрешения пользователя
            return View(microservice);
        }

        // GET: Microservice
        public async Task<IActionResult> Index(int? projectId)
        {
            // Получаем список всех проектов для фильтра
            ViewBag.Projects = new SelectList(await _context.Projects.ToListAsync(), "Id", "Name");

            // Если проект выбран, фильтруем микросервисы по проекту
            var microservices = _context.Microservices
                .Include(m => m.Project)
                .AsQueryable();

            if (projectId.HasValue)
            {
                microservices = microservices.Where(m => m.ProjectId == projectId.Value);
            }

            ViewBag.UserPermissions = GetUserPermissions(User.Identity.Name); // Получаем разрешения пользователя
            // Загружаем микросервисы, связанные с выбранным проектом
            return View(await microservices.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Save(Microservice microservice)
        {
            if (microservice.Id == 0)
            {
                _context.Microservices.Add(microservice);
            }
            else
            {
                _context.Microservices.Update(microservice);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Microservice");
        }




        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var microservice = await _context.Microservices.FindAsync(id);
            if (microservice != null)
            {
                _context.Microservices.Remove(microservice);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Microservice");
        }

    }
}
