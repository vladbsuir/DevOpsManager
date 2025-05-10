using Microsoft.EntityFrameworkCore;
using DevOpsManager.Models;

namespace DevOpsManager.Data
{
    public class DevOpsManagerContext : DbContext
    {
        public DevOpsManagerContext(DbContextOptions<DevOpsManagerContext> options)
            : base(options) { }

        // Таблицы
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Microservice> Microservices { get; set; }
        public DbSet<Deployment> Deployments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Связь User -> Role (многие к одному)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // Связь Project -> User (многие к одному)
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Creator)
                .WithMany(u => u.Projects)
                .HasForeignKey(p => p.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            // Связь Microservice -> Project (многие к одному)
            modelBuilder.Entity<Microservice>()
                .HasOne(m => m.Project)
                .WithMany(p => p.Microservices)
                .HasForeignKey(m => m.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь Deployment -> Microservice (многие к одному)
            modelBuilder.Entity<Deployment>()
                .HasOne(d => d.Microservice)
                .WithMany(m => m.Deployments)
                .HasForeignKey(d => d.MicroserviceId)
                .OnDelete(DeleteBehavior.Cascade);

            // Добавляем начальные роли, если их нет
            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    Id = 1,
                    Name = "Администратор",
                    Permissions =
                        AccessPermissions.Users |
                        AccessPermissions.Projects |
                        AccessPermissions.MicroservicesRead |
                        AccessPermissions.MicroservicesEdit |
                        AccessPermissions.DeployRead |
                        AccessPermissions.DeployEdit
                },

                    new Role
                    {
                        Id = 2,
                        Name = "DevOps-инженер",
                        Permissions =
                        AccessPermissions.Projects |
                        AccessPermissions.MicroservicesRead |
                        AccessPermissions.MicroservicesEdit |
                        AccessPermissions.DeployRead |
                        AccessPermissions.DeployEdit
                    },

                    new Role
                    {
                        Id = 3,
                        Name = "Менеджер",
                        Permissions =
                        AccessPermissions.Projects |
                        AccessPermissions.MicroservicesRead |
                        AccessPermissions.DeployRead
                    },

                    new Role
                    {
                        Id = 4,
                        Name = "Разработчик",
                        Permissions =
                        AccessPermissions.MicroservicesRead |
                        AccessPermissions.MicroservicesEdit |
                        AccessPermissions.DeployRead
                    },

                    new Role
                    {
                        Id = 5,
                        Name = "Гость",
                        Permissions =
                        AccessPermissions.MicroservicesRead |
                        AccessPermissions.DeployRead
                    }
            );
        }
    }
}

