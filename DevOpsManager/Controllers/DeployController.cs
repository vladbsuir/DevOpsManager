using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DevOpsManager.Data;
using DevOpsManager.Models;

namespace DevOpsManager.Controllers
{
    public class DeployController : Controller
    {
        private readonly DevOpsManagerContext _context;

        public DeployController(DevOpsManagerContext context)
        {
            _context = context;
        }

        private AccessPermissions GetUserPermissions(string username)
        {
            var user = _context.Users.Include(u => u.Role)
                                     .FirstOrDefault(u => u.Username == username);

            return user?.Role?.Permissions ?? AccessPermissions.None;
        }

        public async Task<IActionResult> Index(int? projectId, int? microserviceId)
        {
            var deployments = _context.Deployments
                .Include(d => d.Microservice)
                    .ThenInclude(m => m.Project)
                .AsQueryable();

            if (projectId.HasValue)
            {
                deployments = deployments.Where(d => d.Microservice.ProjectId == projectId.Value);
                ViewBag.SelectedProjectId = projectId;
            }

            if (microserviceId.HasValue)
            {
                deployments = deployments.Where(d => d.MicroserviceId == microserviceId.Value);
                ViewBag.SelectedMicroserviceId = microserviceId;
            }

            var projects = await _context.Projects.OrderBy(p => p.Name).ToListAsync();
            var microservices = await _context.Microservices.OrderBy(m => m.Name).ToListAsync();

            ViewBag.Projects = new SelectList(projects, "Id", "Name");
            ViewBag.Microservices = new SelectList(microservices, "Id", "Name");
            ViewBag.UserPermissions = GetUserPermissions(User.Identity?.Name ?? "");

            return View(await deployments.OrderByDescending(d => d.DeployedAt).ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            var microservices = await _context.Microservices.Include(m => m.Project).ToListAsync();
            ViewBag.Microservices = new SelectList(microservices, "Id", "Name");
            ViewBag.UserPermissions = GetUserPermissions(User.Identity?.Name ?? "");

            if (id == null)
            {
                return View(new Deployment
                {
                    DeployedAt = DateTime.UtcNow,
                    DeployedBy = User.Identity?.Name ?? "system"
                });
            }

            var deploy = await _context.Deployments.FindAsync(id);
            if (deploy == null) return NotFound();
            return View(deploy);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Deployment model)
        {
            if (model.Id == 0)
            {
                model.DeployedAt = DateTime.UtcNow;
                model.DeployedBy = User.Identity?.Name ?? "system";
                _context.Deployments.Add(model);
            }
            else
            {
                model.DeployedAt = model.DeployedAt.ToUniversalTime();
                _context.Deployments.Update(model);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Deploy");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var deployment = await _context.Deployments.FindAsync(id);
            if (deployment == null) return NotFound();

            _context.Deployments.Remove(deployment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Deploy");
        }
    }
}
