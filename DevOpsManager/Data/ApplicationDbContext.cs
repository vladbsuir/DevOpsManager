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
                .HasOne(p => p.Owner)
                .WithMany(u => u.Projects)
                .HasForeignKey(p => p.OwnerId)
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

            modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Username = "valakhanovich",
                PasswordHash = "$2a$11$/m/WVwB7JX2gQHd0eHxbFOFWDQzNvVoBC9G6HzuiLYA1.tBbMFJ9i",
                Email = "admin@example.com",
                RoleId = 1,
                CreatedAt = new DateTime(2024, 3, 15).ToUniversalTime(),
                LastLoginAt = new DateTime(2025, 5, 9).ToUniversalTime(),
                FullName = "Валаханович Владислав Александрович",
                SlackHandle = "@valakhanovich",
                PhoneNumber = "+375291000001",
                Position = "Системный администратор",
                Department = "ИТ-отдел",
                Location = "Минск",
                BirthDate = new DateTime(1990, 1, 5).ToUniversalTime()
            },
            new User
            {
                Id = 2,
                Username = "egorova",
                PasswordHash = "$2a$11$/m/WVwB7JX2gQHd0eHxbFOFWDQzNvVoBC9G6HzuiLYA1.tBbMFJ9i",
                Email = "e.egorova@example.com",
                RoleId = 5,
                CreatedAt = new DateTime(2024, 8, 22).ToUniversalTime(),
                LastLoginAt = new DateTime(2025, 4, 20).ToUniversalTime(),
                FullName = "Андреева Наталья Олеговна",
                SlackHandle = "@egorova",
                PhoneNumber = "+375291000002",
                Position = "Менеджер проектов",
                Department = "PMO",
                Location = "Минск",
                BirthDate = new DateTime(1992, 6, 10).ToUniversalTime()
            },
            new User
            {
                Id = 3,
                Username = "dmitriev",
                PasswordHash = "$2a$11$/m/WVwB7JX2gQHd0eHxbFOFWDQzNvVoBC9G6HzuiLYA1.tBbMFJ9i",
                Email = "a.dmitriev@example.com",
                RoleId = 5,
                CreatedAt = new DateTime(2024, 5, 12).ToUniversalTime(),
                LastLoginAt = new DateTime(2025, 1, 4).ToUniversalTime(),
                FullName = "Дмитриев Алексей Игоревич",
                SlackHandle = "@dmitriev",
                PhoneNumber = "+375291000003",
                Position = "DevOps-инженер",
                Department = "Инфраструктура",
                Location = "Брест",
                BirthDate = new DateTime(1991, 3, 11).ToUniversalTime()
            },
            new User
            {
                Id = 4,
                Username = "kuznetsova",
                PasswordHash = "$2a$11$/m/WVwB7JX2gQHd0eHxbFOFWDQzNvVoBC9G6HzuiLYA1.tBbMFJ9i",
                Email = "n.kuznetsova@example.com",
                RoleId = 5,
                CreatedAt = new DateTime(2024, 10, 5).ToUniversalTime(),
                LastLoginAt = null,
                FullName = "Кузнецова Надежда Павловна",
                SlackHandle = "@kuznetsova",
                PhoneNumber = "+375291000004",
                Position = "Аналитик",
                Department = "Аналитика",
                Location = "Гомель",
                BirthDate = new DateTime(1994, 9, 18).ToUniversalTime()
            },
            new User
            {
                Id = 5,
                Username = "petrov",
                PasswordHash = "$2a$11$/m/WVwB7JX2gQHd0eHxbFOFWDQzNvVoBC9G6HzuiLYA1.tBbMFJ9i",
                Email = "p.petrov@example.com",
                RoleId = 5,
                CreatedAt = new DateTime(2024, 7, 19).ToUniversalTime(),
                LastLoginAt = new DateTime(2025, 5, 1).ToUniversalTime(),
                FullName = "Петров Владимир Степанович",
                SlackHandle = "@petrov",
                PhoneNumber = "+375291000005",
                Position = "Backend-разработчик",
                Department = "Разработка",
                Location = "Минск",
                BirthDate = new DateTime(1993, 12, 2).ToUniversalTime()
            },
            new User
            {
                Id = 6,
                Username = "sidorova",
                PasswordHash = "$2a$11$/m/WVwB7JX2gQHd0eHxbFOFWDQzNvVoBC9G6HzuiLYA1.tBbMFJ9i",
                Email = "o.sidorova@example.com",
                RoleId = 5,
                CreatedAt = new DateTime(2024, 1, 30).ToUniversalTime(),
                LastLoginAt = new DateTime(2025, 3, 8).ToUniversalTime(),
                FullName = "Сидорова Ольга Сергеевна",
                SlackHandle = "@sidorova",
                PhoneNumber = "+375291000006",
                Position = "Бизнес-аналитик",
                Department = "Аналитика",
                Location = "Витебск",
                BirthDate = new DateTime(1995, 2, 17).ToUniversalTime()
            },
            new User
            {
                Id = 7,
                Username = "karpov",
                PasswordHash = "$2a$11$/m/WVwB7JX2gQHd0eHxbFOFWDQzNvVoBC9G6HzuiLYA1.tBbMFJ9i",
                Email = "v.karpov@example.com",
                RoleId = 5,
                CreatedAt = new DateTime(2024, 6, 11).ToUniversalTime(),
                LastLoginAt = null,
                FullName = "Карпов Виктор Николаевич",
                SlackHandle = "@karpov",
                PhoneNumber = "+375291000007",
                Position = "Тестировщик",
                Department = "QA",
                Location = "Минск",
                BirthDate = new DateTime(1996, 11, 22).ToUniversalTime()
            },
            new User
            {
                Id = 8,
                Username = "zaytseva",
                PasswordHash = "$2a$11$/m/WVwB7JX2gQHd0eHxbFOFWDQzNvVoBC9G6HzuiLYA1.tBbMFJ9i",
                Email = "m.zaytseva@example.com",
                RoleId = 5,
                CreatedAt = new DateTime(2024, 9, 5).ToUniversalTime(),
                LastLoginAt = new DateTime(2025, 5, 10).ToUniversalTime(),
                FullName = "Зайцева Марина Юрьевна",
                SlackHandle = "@zaytseva",
                PhoneNumber = "+375291000008",
                Position = "BI-специалист",
                Department = "BI",
                Location = "Минск",
                BirthDate = new DateTime(1992, 4, 25).ToUniversalTime()
            },
            new User
            {
                Id = 9,
                Username = "novikov",
                PasswordHash = "$2a$11$/m/WVwB7JX2gQHd0eHxbFOFWDQzNvVoBC9G6HzuiLYA1.tBbMFJ9i",
                Email = "k.novikov@example.com",
                RoleId = 5,
                CreatedAt = new DateTime(2024, 12, 10).ToUniversalTime(),
                LastLoginAt = new DateTime(2025, 4, 1).ToUniversalTime(),
                FullName = "Новиков Константин Аркадьевич",
                SlackHandle = "@novikov",
                PhoneNumber = "+375291000009",
                Position = "Руководитель группы",
                Department = "Разработка",
                Location = "Минск",
                BirthDate = new DateTime(1989, 5, 8).ToUniversalTime()
            },
            new User
            {
                Id = 10,
                Username = "smirnov",
                PasswordHash = "$2a$11$/m/WVwB7JX2gQHd0eHxbFOFWDQzNvVoBC9G6HzuiLYA1.tBbMFJ9i",
                Email = "d.smirnov@example.com",
                RoleId = 5,
                CreatedAt = new DateTime(2024, 2, 14).ToUniversalTime(),
                LastLoginAt = null,
                FullName = "Смирнов Дмитрий Иванович",
                SlackHandle = "@smirnov",
                PhoneNumber = "+375291000010",
                Position = "Стажёр DevOps",
                Department = "Инфраструктура",
                Location = "Минск",
                BirthDate = new DateTime(2000, 8, 3).ToUniversalTime()
            }
        );

            modelBuilder.Entity<Project>().HasData(
                new Project
                {
                    Id = 1,
                    Name = "CI/CD Pipeline",
                    Code = "CI-CD",
                    Description = "Автоматизация CI/CD процессов",
                    JiraUrl = "https://jira.example.com/projects/CICD",
                    ConfluenceUrl = "https://confluence.example.com/display/CICD",
                    GitRepository = "https://git.example.com/devops/cicd-pipeline",
                    SlackChannel = "#cicd",
                    StartDate = new DateTime(2023, 2, 10).ToUniversalTime(),
                    Status = "Активен",
                    Environment = "Prod",
                    DeploymentUrl = "https://deploy.example.com/cicd",
                    OwnerId = 2
                },
                new Project
                {
                    Id = 2,
                    Name = "Monitoring System",
                    Code = "MONSYS",
                    Description = "Разработка системы мониторинга и алертинга",
                    JiraUrl = "https://jira.example.com/projects/MONSYS",
                    ConfluenceUrl = "https://confluence.example.com/display/MONSYS",
                    GitRepository = "https://git.example.com/devops/monitoring",
                    SlackChannel = "#monitoring",
                    StartDate = new DateTime(2022, 11, 1).ToUniversalTime(),
                    Status = "Активен",
                    Environment = "Prod",
                    DeploymentUrl = "https://deploy.example.com/monitoring",
                    OwnerId = 3
                },
                new Project
                {
                    Id = 3,
                    Name = "API Gateway",
                    Code = "APIGW",
                    Description = "Единая точка входа для микросервисов",
                    JiraUrl = "https://jira.example.com/projects/APIGW",
                    ConfluenceUrl = "https://confluence.example.com/display/APIGW",
                    GitRepository = "https://git.example.com/backend/api-gateway",
                    SlackChannel = "#api-gateway",
                    StartDate = new DateTime(2023, 5, 5).ToUniversalTime(),
                    Status = "Активен",
                    Environment = "Staging",
                    DeploymentUrl = "https://deploy.example.com/api-gw",
                    OwnerId = 4
                },
                new Project
                {
                    Id = 4,
                    Name = "User Management",
                    Code = "USRMGT",
                    Description = "Управление пользователями и ролями",
                    JiraUrl = "https://jira.example.com/projects/USRMGT",
                    ConfluenceUrl = "https://confluence.example.com/display/USRMGT",
                    GitRepository = "https://git.example.com/backend/user-service",
                    SlackChannel = "#user-mgmt",
                    StartDate = new DateTime(2021, 8, 15).ToUniversalTime(),
                    Status = "Завершён",
                    Environment = "Prod",
                    DeploymentUrl = "https://deploy.example.com/user-mgmt",
                    OwnerId = 1
                },
                new Project
                {
                    Id = 5,
                    Name = "Billing Service",
                    Code = "BILL",
                    Description = "Сервис расчёта и выставления счетов",
                    JiraUrl = "https://jira.example.com/projects/BILL",
                    ConfluenceUrl = "https://confluence.example.com/display/BILL",
                    GitRepository = "https://git.example.com/backend/billing-service",
                    SlackChannel = "#billing",
                    StartDate = new DateTime(2022, 3, 20).ToUniversalTime(),
                    Status = "Активен",
                    Environment = "Dev",
                    DeploymentUrl = "https://deploy.example.com/billing",
                    OwnerId = 5
                },
                new Project
                {
                    Id = 6,
                    Name = "Notification System",
                    Code = "NOTIFY",
                    Description = "Отправка уведомлений пользователям",
                    JiraUrl = "https://jira.example.com/projects/NOTIFY",
                    ConfluenceUrl = "https://confluence.example.com/display/NOTIFY",
                    GitRepository = "https://git.example.com/devops/notifications",
                    SlackChannel = "#notifications",
                    StartDate = new DateTime(2023, 1, 1).ToUniversalTime(),
                    Status = "Активен",
                    Environment = "Staging",
                    DeploymentUrl = "https://deploy.example.com/notify",
                    OwnerId = 6
                },
                new Project
                {
                    Id = 7,
                    Name = "Analytics Platform",
                    Code = "ANALYTICS",
                    Description = "Платформа сбора и анализа данных",
                    JiraUrl = "https://jira.example.com/projects/ANALYTICS",
                    ConfluenceUrl = "https://confluence.example.com/display/ANALYTICS",
                    GitRepository = "https://git.example.com/bi/analytics",
                    SlackChannel = "#analytics",
                    StartDate = new DateTime(2023, 9, 17).ToUniversalTime(),
                    Status = "Активен",
                    Environment = "Prod",
                    DeploymentUrl = "https://deploy.example.com/analytics",
                    OwnerId = 1
                },
                new Project
                {
                    Id = 8,
                    Name = "Authentication Service",
                    Code = "AUTH",
                    Description = "Сервис аутентификации и авторизации",
                    JiraUrl = "https://jira.example.com/projects/AUTH",
                    ConfluenceUrl = "https://confluence.example.com/display/AUTH",
                    GitRepository = "https://git.example.com/security/auth-service",
                    SlackChannel = "#auth",
                    StartDate = new DateTime(2023, 4, 10).ToUniversalTime(),
                    Status = "Активен",
                    Environment = "Prod",
                    DeploymentUrl = "https://deploy.example.com/auth",
                    OwnerId = 2
                },
                new Project
                {
                    Id = 9,
                    Name = "Frontend Portal",
                    Code = "PORTAL",
                    Description = "Веб-портал для конечных пользователей",
                    JiraUrl = "https://jira.example.com/projects/PORTAL",
                    ConfluenceUrl = "https://confluence.example.com/display/PORTAL",
                    GitRepository = "https://git.example.com/frontend/portal",
                    SlackChannel = "#frontend",
                    StartDate = new DateTime(2022, 6, 18).ToUniversalTime(),
                    Status = "Приостановлен",
                    Environment = "Staging",
                    DeploymentUrl = "https://deploy.example.com/portal",
                    OwnerId = 7
                },
                new Project
                {
                    Id = 10,
                    Name = "Internal Tools",
                    Code = "TOOLS",
                    Description = "Внутренние инструменты команды DevOps",
                    JiraUrl = "https://jira.example.com/projects/TOOLS",
                    ConfluenceUrl = "https://confluence.example.com/display/TOOLS",
                    GitRepository = "https://git.example.com/devops/tools",
                    SlackChannel = "#internal-tools",
                    StartDate = new DateTime(2021, 10, 12).ToUniversalTime(),
                    Status = "Активен",
                    Environment = "Dev",
                    DeploymentUrl = "https://deploy.example.com/tools",
                    OwnerId = 9
                }
            );
            modelBuilder.Entity<Microservice>().HasData(
                new Microservice
                {
                    Id = 1,
                    Name = "CI/CD Orchestrator",
                    ImageUrl = "registry.example.com/cicd/orchestrator:latest",
                    Environment = "Prod",
                    ProjectId = 1,
                    GrafanaPanelUrl = "https://grafana.example.com/d/cicd",
                    LogsUrl = "https://logs.example.com/cicd/orchestrator",
                    HealthcheckEndpoint = "https://cicd.example.com/health"
                },
                new Microservice
                {
                    Id = 2,
                    Name = "Monitoring Collector",
                    ImageUrl = "registry.example.com/monitoring/collector:1.0.0",
                    Environment = "Prod",
                    ProjectId = 2,
                    GrafanaPanelUrl = "https://grafana.example.com/d/monitoring",
                    LogsUrl = "https://logs.example.com/monitoring/collector",
                    HealthcheckEndpoint = "https://monitoring.example.com/collector/health"
                },
                new Microservice
                {
                    Id = 3,
                    Name = "Monitoring Notifier",
                    ImageUrl = "registry.example.com/monitoring/notifier:1.0.0",
                    Environment = "Prod",
                    ProjectId = 2,
                    GrafanaPanelUrl = "https://grafana.example.com/d/monitoring",
                    LogsUrl = "https://logs.example.com/monitoring/notifier",
                    HealthcheckEndpoint = "https://monitoring.example.com/notifier/health"
                },
                new Microservice
                {
                    Id = 4,
                    Name = "API Gateway Core",
                    ImageUrl = "registry.example.com/api-gw/core:2.1.0",
                    Environment = "Staging",
                    ProjectId = 3,
                    GrafanaPanelUrl = "https://grafana.example.com/d/apigw",
                    LogsUrl = "https://logs.example.com/apigw/core",
                    HealthcheckEndpoint = "https://apigw.example.com/health"
                },
                new Microservice
                {
                    Id = 5,
                    Name = "User API",
                    ImageUrl = "registry.example.com/user/api:1.0.1",
                    Environment = "Prod",
                    ProjectId = 4,
                    GrafanaPanelUrl = "https://grafana.example.com/d/user",
                    LogsUrl = "https://logs.example.com/user/api",
                    HealthcheckEndpoint = "https://user.example.com/api/health"
                },
                new Microservice
                {
                    Id = 6,
                    Name = "Billing Core",
                    ImageUrl = "registry.example.com/billing/core:3.0.0",
                    Environment = "Dev",
                    ProjectId = 5,
                    GrafanaPanelUrl = "https://grafana.example.com/d/billing",
                    LogsUrl = "https://logs.example.com/billing/core",
                    HealthcheckEndpoint = "https://billing.example.com/core/health"
                },
                new Microservice
                {
                    Id = 7,
                    Name = "Notification Sender",
                    ImageUrl = "registry.example.com/notify/sender:1.2.0",
                    Environment = "Staging",
                    ProjectId = 6,
                    GrafanaPanelUrl = "https://grafana.example.com/d/notify",
                    LogsUrl = "https://logs.example.com/notify/sender",
                    HealthcheckEndpoint = "https://notify.example.com/sender/health"
                },
                new Microservice
                {
                    Id = 8,
                    Name = "Analytics Ingest",
                    ImageUrl = "registry.example.com/analytics/ingest:2.0.0",
                    Environment = "Prod",
                    ProjectId = 7,
                    GrafanaPanelUrl = "https://grafana.example.com/d/analytics",
                    LogsUrl = "https://logs.example.com/analytics/ingest",
                    HealthcheckEndpoint = "https://analytics.example.com/ingest/health"
                },
                new Microservice
                {
                    Id = 9,
                    Name = "Auth Core",
                    ImageUrl = "registry.example.com/auth/core:1.3.0",
                    Environment = "Prod",
                    ProjectId = 8,
                    GrafanaPanelUrl = "https://grafana.example.com/d/auth",
                    LogsUrl = "https://logs.example.com/auth/core",
                    HealthcheckEndpoint = "https://auth.example.com/core/health"
                },
                new Microservice
                {
                    Id = 10,
                    Name = "Frontend UI",
                    ImageUrl = "registry.example.com/portal/frontend:1.0.0",
                    Environment = "Staging",
                    ProjectId = 9,
                    GrafanaPanelUrl = "https://grafana.example.com/d/portal",
                    LogsUrl = "https://logs.example.com/portal/frontend",
                    HealthcheckEndpoint = "https://portal.example.com/health"
                },
                new Microservice
                {
                    Id = 11,
                    Name = "DevOps Helper",
                    ImageUrl = "registry.example.com/tools/helper:0.9.5",
                    Environment = "Dev",
                    ProjectId = 10,
                    GrafanaPanelUrl = "https://grafana.example.com/d/tools",
                    LogsUrl = "https://logs.example.com/tools/helper",
                    HealthcheckEndpoint = "https://tools.example.com/helper/health"
                }
            );

            modelBuilder.Entity<Deployment>().HasData(
                new Deployment
                {
                    Id = 1,
                    MicroserviceId = 1,
                    Version = "1.0.0",
                    DeployedAt = new DateTime(2025, 05, 01, 14, 30, 0).ToUniversalTime(),
                    Environment = "Prod",
                    DeployedBy = "valakhanovich",
                    Status = "Success",
                    RemoteLogsUrl = "https://logs.example.com/cicd/orchestrator/deploy/1.0.0"
                },
                new Deployment
                {
                    Id = 2,
                    MicroserviceId = 2,
                    Version = "1.0.0",
                    DeployedAt = new DateTime(2025, 05, 02, 10, 15, 0).ToUniversalTime(),
                    Environment = "Prod",
                    DeployedBy = "kirilov",
                    Status = "Success",
                    RemoteLogsUrl = "https://logs.example.com/monitoring/collector/deploy/1.0.0"
                },
                new Deployment
                {
                    Id = 3,
                    MicroserviceId = 3,
                    Version = "1.0.0",
                    DeployedAt = new DateTime(2025, 05, 02, 10, 20, 0).ToUniversalTime(),
                    Environment = "Prod",
                    DeployedBy = "kirilov",
                    Status = "Success",
                    RemoteLogsUrl = "https://logs.example.com/monitoring/notifier/deploy/1.0.0"
                },
                new Deployment
                {
                    Id = 4,
                    MicroserviceId = 4,
                    Version = "2.1.0",
                    DeployedAt = new DateTime(2025, 04, 28, 17, 0, 0).ToUniversalTime(),
                    Environment = "Staging",
                    DeployedBy = "dronov",
                    Status = "Success",
                    RemoteLogsUrl = "https://logs.example.com/apigw/core/deploy/2.1.0"
                },
                new Deployment
                {
                    Id = 5,
                    MicroserviceId = 5,
                    Version = "1.0.1",
                    DeployedAt = new DateTime(2025, 05, 05, 09, 45, 0).ToUniversalTime(),
                    Environment = "Prod",
                    DeployedBy = "ivanchenko",
                    Status = "Success",
                    RemoteLogsUrl = "https://logs.example.com/user/api/deploy/1.0.1"
                },
                new Deployment
                {
                    Id = 6,
                    MicroserviceId = 6,
                    Version = "3.0.0-beta",
                    DeployedAt = new DateTime(2025, 05, 07, 13, 0, 0).ToUniversalTime(),
                    Environment = "Dev",
                    DeployedBy = "sergeev",
                    Status = "Success",
                    RemoteLogsUrl = "https://logs.example.com/billing/core/deploy/3.0.0-beta"
                },
                new Deployment
                {
                    Id = 7,
                    MicroserviceId = 7,
                    Version = "1.2.0",
                    DeployedAt = new DateTime(2025, 05, 06, 11, 30, 0).ToUniversalTime(),
                    Environment = "Staging",
                    DeployedBy = "stepanova",
                    Status = "Failed",
                    RemoteLogsUrl = "https://logs.example.com/notify/sender/deploy/1.2.0"
                },
                new Deployment
                {
                    Id = 8,
                    MicroserviceId = 8,
                    Version = "2.0.0",
                    DeployedAt = new DateTime(2025, 05, 03, 15, 10, 0).ToUniversalTime(),
                    Environment = "Prod",
                    DeployedBy = "nikitin",
                    Status = "Success",
                    RemoteLogsUrl = "https://logs.example.com/analytics/ingest/deploy/2.0.0"
                },
                new Deployment
                {
                    Id = 9,
                    MicroserviceId = 9,
                    Version = "1.3.0",
                    DeployedAt = new DateTime(2025, 05, 04, 18, 0, 0).ToUniversalTime(),
                    Environment = "Prod",
                    DeployedBy = "andreeva",
                    Status = "Success",
                    RemoteLogsUrl = "https://logs.example.com/auth/core/deploy/1.3.0"
                },
                new Deployment
                {
                    Id = 10,
                    MicroserviceId = 10,
                    Version = "1.0.0-rc",
                    DeployedAt = new DateTime(2025, 05, 08, 12, 0, 0).ToUniversalTime(),
                    Environment = "Staging",
                    DeployedBy = "kozlova",
                    Status = "Success",
                    RemoteLogsUrl = "https://logs.example.com/portal/frontend/deploy/1.0.0-rc"
                }
            );

        }
    }
}

