using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DevOpsManager.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Permissions = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    SlackHandle = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    Position = table.Column<string>(type: "text", nullable: false),
                    Department = table.Column<string>(type: "text", nullable: false),
                    Location = table.Column<string>(type: "text", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    JiraUrl = table.Column<string>(type: "text", nullable: false),
                    ConfluenceUrl = table.Column<string>(type: "text", nullable: false),
                    GitRepository = table.Column<string>(type: "text", nullable: false),
                    SlackChannel = table.Column<string>(type: "text", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false),
                    Environment = table.Column<string>(type: "text", nullable: false),
                    DeploymentUrl = table.Column<string>(type: "text", nullable: false),
                    OwnerId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Microservices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: false),
                    Environment = table.Column<string>(type: "text", nullable: false),
                    ProjectId = table.Column<int>(type: "integer", nullable: false),
                    GrafanaPanelUrl = table.Column<string>(type: "text", nullable: false),
                    LogsUrl = table.Column<string>(type: "text", nullable: false),
                    HealthcheckEndpoint = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Microservices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Microservices_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Deployments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MicroserviceId = table.Column<int>(type: "integer", nullable: false),
                    Version = table.Column<string>(type: "text", nullable: false),
                    DeployedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Environment = table.Column<string>(type: "text", nullable: false),
                    DeployedBy = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    RemoteLogsUrl = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deployments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Deployments_Microservices_MicroserviceId",
                        column: x => x.MicroserviceId,
                        principalTable: "Microservices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name", "Permissions" },
                values: new object[,]
                {
                    { 1, "Администратор", 63 },
                    { 2, "DevOps-инженер", 62 },
                    { 3, "Менеджер", 22 },
                    { 4, "Разработчик", 28 },
                    { 5, "Гость", 20 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BirthDate", "CreatedAt", "Department", "Email", "FullName", "LastLoginAt", "Location", "PasswordHash", "PhoneNumber", "Position", "RoleId", "SlackHandle", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(1990, 1, 4, 22, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 3, 14, 21, 0, 0, 0, DateTimeKind.Utc), "ИТ-отдел", "admin@example.com", "Валаханович Владислав Александрович", new DateTime(2025, 5, 8, 21, 0, 0, 0, DateTimeKind.Utc), "Минск", "$2a$11$/m/WVwB7JX2gQHd0eHxbFOFWDQzNvVoBC9G6HzuiLYA1.tBbMFJ9i", "+375291000001", "Системный администратор", 1, "@valakhanovich", "valakhanovich" },
                    { 2, new DateTime(1992, 6, 9, 21, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 8, 21, 21, 0, 0, 0, DateTimeKind.Utc), "PMO", "e.egorova@example.com", "Андреева Наталья Олеговна", new DateTime(2025, 4, 19, 21, 0, 0, 0, DateTimeKind.Utc), "Минск", "$2a$11$/m/WVwB7JX2gQHd0eHxbFOFWDQzNvVoBC9G6HzuiLYA1.tBbMFJ9i", "+375291000002", "Менеджер проектов", 5, "@egorova", "egorova" },
                    { 3, new DateTime(1991, 3, 10, 22, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 5, 11, 21, 0, 0, 0, DateTimeKind.Utc), "Инфраструктура", "a.dmitriev@example.com", "Дмитриев Алексей Игоревич", new DateTime(2025, 1, 3, 21, 0, 0, 0, DateTimeKind.Utc), "Брест", "$2a$11$/m/WVwB7JX2gQHd0eHxbFOFWDQzNvVoBC9G6HzuiLYA1.tBbMFJ9i", "+375291000003", "DevOps-инженер", 5, "@dmitriev", "dmitriev" },
                    { 4, new DateTime(1994, 9, 17, 21, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 10, 4, 21, 0, 0, 0, DateTimeKind.Utc), "Аналитика", "n.kuznetsova@example.com", "Кузнецова Надежда Павловна", null, "Гомель", "$2a$11$/m/WVwB7JX2gQHd0eHxbFOFWDQzNvVoBC9G6HzuiLYA1.tBbMFJ9i", "+375291000004", "Аналитик", 5, "@kuznetsova", "kuznetsova" },
                    { 5, new DateTime(1993, 12, 1, 22, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 7, 18, 21, 0, 0, 0, DateTimeKind.Utc), "Разработка", "p.petrov@example.com", "Петров Владимир Степанович", new DateTime(2025, 4, 30, 21, 0, 0, 0, DateTimeKind.Utc), "Минск", "$2a$11$/m/WVwB7JX2gQHd0eHxbFOFWDQzNvVoBC9G6HzuiLYA1.tBbMFJ9i", "+375291000005", "Backend-разработчик", 5, "@petrov", "petrov" },
                    { 6, new DateTime(1995, 2, 16, 22, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 29, 21, 0, 0, 0, DateTimeKind.Utc), "Аналитика", "o.sidorova@example.com", "Сидорова Ольга Сергеевна", new DateTime(2025, 3, 7, 21, 0, 0, 0, DateTimeKind.Utc), "Витебск", "$2a$11$/m/WVwB7JX2gQHd0eHxbFOFWDQzNvVoBC9G6HzuiLYA1.tBbMFJ9i", "+375291000006", "Бизнес-аналитик", 5, "@sidorova", "sidorova" },
                    { 7, new DateTime(1996, 11, 21, 22, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 6, 10, 21, 0, 0, 0, DateTimeKind.Utc), "QA", "v.karpov@example.com", "Карпов Виктор Николаевич", null, "Минск", "$2a$11$/m/WVwB7JX2gQHd0eHxbFOFWDQzNvVoBC9G6HzuiLYA1.tBbMFJ9i", "+375291000007", "Тестировщик", 5, "@karpov", "karpov" },
                    { 8, new DateTime(1992, 4, 24, 21, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 9, 4, 21, 0, 0, 0, DateTimeKind.Utc), "BI", "m.zaytseva@example.com", "Зайцева Марина Юрьевна", new DateTime(2025, 5, 9, 21, 0, 0, 0, DateTimeKind.Utc), "Минск", "$2a$11$/m/WVwB7JX2gQHd0eHxbFOFWDQzNvVoBC9G6HzuiLYA1.tBbMFJ9i", "+375291000008", "BI-специалист", 5, "@zaytseva", "zaytseva" },
                    { 9, new DateTime(1989, 5, 7, 21, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 12, 9, 21, 0, 0, 0, DateTimeKind.Utc), "Разработка", "k.novikov@example.com", "Новиков Константин Аркадьевич", new DateTime(2025, 3, 31, 21, 0, 0, 0, DateTimeKind.Utc), "Минск", "$2a$11$/m/WVwB7JX2gQHd0eHxbFOFWDQzNvVoBC9G6HzuiLYA1.tBbMFJ9i", "+375291000009", "Руководитель группы", 5, "@novikov", "novikov" },
                    { 10, new DateTime(2000, 8, 2, 21, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 2, 13, 21, 0, 0, 0, DateTimeKind.Utc), "Инфраструктура", "d.smirnov@example.com", "Смирнов Дмитрий Иванович", null, "Минск", "$2a$11$/m/WVwB7JX2gQHd0eHxbFOFWDQzNvVoBC9G6HzuiLYA1.tBbMFJ9i", "+375291000010", "Стажёр DevOps", 5, "@smirnov", "smirnov" }
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "Code", "ConfluenceUrl", "DeploymentUrl", "Description", "EndDate", "Environment", "GitRepository", "JiraUrl", "Name", "OwnerId", "SlackChannel", "StartDate", "Status" },
                values: new object[,]
                {
                    { 1, "CI-CD", "https://confluence.example.com/display/CICD", "https://deploy.example.com/cicd", "Автоматизация CI/CD процессов", null, "Prod", "https://git.example.com/devops/cicd-pipeline", "https://jira.example.com/projects/CICD", "CI/CD Pipeline", 2, "#cicd", new DateTime(2023, 2, 9, 21, 0, 0, 0, DateTimeKind.Utc), "Активен" },
                    { 2, "MONSYS", "https://confluence.example.com/display/MONSYS", "https://deploy.example.com/monitoring", "Разработка системы мониторинга и алертинга", null, "Prod", "https://git.example.com/devops/monitoring", "https://jira.example.com/projects/MONSYS", "Monitoring System", 3, "#monitoring", new DateTime(2022, 10, 31, 21, 0, 0, 0, DateTimeKind.Utc), "Активен" },
                    { 3, "APIGW", "https://confluence.example.com/display/APIGW", "https://deploy.example.com/api-gw", "Единая точка входа для микросервисов", null, "Staging", "https://git.example.com/backend/api-gateway", "https://jira.example.com/projects/APIGW", "API Gateway", 4, "#api-gateway", new DateTime(2023, 5, 4, 21, 0, 0, 0, DateTimeKind.Utc), "Активен" },
                    { 4, "USRMGT", "https://confluence.example.com/display/USRMGT", "https://deploy.example.com/user-mgmt", "Управление пользователями и ролями", null, "Prod", "https://git.example.com/backend/user-service", "https://jira.example.com/projects/USRMGT", "User Management", 1, "#user-mgmt", new DateTime(2021, 8, 14, 21, 0, 0, 0, DateTimeKind.Utc), "Завершён" },
                    { 5, "BILL", "https://confluence.example.com/display/BILL", "https://deploy.example.com/billing", "Сервис расчёта и выставления счетов", null, "Dev", "https://git.example.com/backend/billing-service", "https://jira.example.com/projects/BILL", "Billing Service", 5, "#billing", new DateTime(2022, 3, 19, 21, 0, 0, 0, DateTimeKind.Utc), "Активен" },
                    { 6, "NOTIFY", "https://confluence.example.com/display/NOTIFY", "https://deploy.example.com/notify", "Отправка уведомлений пользователям", null, "Staging", "https://git.example.com/devops/notifications", "https://jira.example.com/projects/NOTIFY", "Notification System", 6, "#notifications", new DateTime(2022, 12, 31, 21, 0, 0, 0, DateTimeKind.Utc), "Активен" },
                    { 7, "ANALYTICS", "https://confluence.example.com/display/ANALYTICS", "https://deploy.example.com/analytics", "Платформа сбора и анализа данных", null, "Prod", "https://git.example.com/bi/analytics", "https://jira.example.com/projects/ANALYTICS", "Analytics Platform", 1, "#analytics", new DateTime(2023, 9, 16, 21, 0, 0, 0, DateTimeKind.Utc), "Активен" },
                    { 8, "AUTH", "https://confluence.example.com/display/AUTH", "https://deploy.example.com/auth", "Сервис аутентификации и авторизации", null, "Prod", "https://git.example.com/security/auth-service", "https://jira.example.com/projects/AUTH", "Authentication Service", 2, "#auth", new DateTime(2023, 4, 9, 21, 0, 0, 0, DateTimeKind.Utc), "Активен" },
                    { 9, "PORTAL", "https://confluence.example.com/display/PORTAL", "https://deploy.example.com/portal", "Веб-портал для конечных пользователей", null, "Staging", "https://git.example.com/frontend/portal", "https://jira.example.com/projects/PORTAL", "Frontend Portal", 7, "#frontend", new DateTime(2022, 6, 17, 21, 0, 0, 0, DateTimeKind.Utc), "Приостановлен" },
                    { 10, "TOOLS", "https://confluence.example.com/display/TOOLS", "https://deploy.example.com/tools", "Внутренние инструменты команды DevOps", null, "Dev", "https://git.example.com/devops/tools", "https://jira.example.com/projects/TOOLS", "Internal Tools", 9, "#internal-tools", new DateTime(2021, 10, 11, 21, 0, 0, 0, DateTimeKind.Utc), "Активен" }
                });

            migrationBuilder.InsertData(
                table: "Microservices",
                columns: new[] { "Id", "Environment", "GrafanaPanelUrl", "HealthcheckEndpoint", "ImageUrl", "LogsUrl", "Name", "ProjectId" },
                values: new object[,]
                {
                    { 1, "Prod", "https://grafana.example.com/d/cicd", "https://cicd.example.com/health", "registry.example.com/cicd/orchestrator:latest", "https://logs.example.com/cicd/orchestrator", "CI/CD Orchestrator", 1 },
                    { 2, "Prod", "https://grafana.example.com/d/monitoring", "https://monitoring.example.com/collector/health", "registry.example.com/monitoring/collector:1.0.0", "https://logs.example.com/monitoring/collector", "Monitoring Collector", 2 },
                    { 3, "Prod", "https://grafana.example.com/d/monitoring", "https://monitoring.example.com/notifier/health", "registry.example.com/monitoring/notifier:1.0.0", "https://logs.example.com/monitoring/notifier", "Monitoring Notifier", 2 },
                    { 4, "Staging", "https://grafana.example.com/d/apigw", "https://apigw.example.com/health", "registry.example.com/api-gw/core:2.1.0", "https://logs.example.com/apigw/core", "API Gateway Core", 3 },
                    { 5, "Prod", "https://grafana.example.com/d/user", "https://user.example.com/api/health", "registry.example.com/user/api:1.0.1", "https://logs.example.com/user/api", "User API", 4 },
                    { 6, "Dev", "https://grafana.example.com/d/billing", "https://billing.example.com/core/health", "registry.example.com/billing/core:3.0.0", "https://logs.example.com/billing/core", "Billing Core", 5 },
                    { 7, "Staging", "https://grafana.example.com/d/notify", "https://notify.example.com/sender/health", "registry.example.com/notify/sender:1.2.0", "https://logs.example.com/notify/sender", "Notification Sender", 6 },
                    { 8, "Prod", "https://grafana.example.com/d/analytics", "https://analytics.example.com/ingest/health", "registry.example.com/analytics/ingest:2.0.0", "https://logs.example.com/analytics/ingest", "Analytics Ingest", 7 },
                    { 9, "Prod", "https://grafana.example.com/d/auth", "https://auth.example.com/core/health", "registry.example.com/auth/core:1.3.0", "https://logs.example.com/auth/core", "Auth Core", 8 },
                    { 10, "Staging", "https://grafana.example.com/d/portal", "https://portal.example.com/health", "registry.example.com/portal/frontend:1.0.0", "https://logs.example.com/portal/frontend", "Frontend UI", 9 },
                    { 11, "Dev", "https://grafana.example.com/d/tools", "https://tools.example.com/helper/health", "registry.example.com/tools/helper:0.9.5", "https://logs.example.com/tools/helper", "DevOps Helper", 10 }
                });

            migrationBuilder.InsertData(
                table: "Deployments",
                columns: new[] { "Id", "DeployedAt", "DeployedBy", "Environment", "MicroserviceId", "RemoteLogsUrl", "Status", "Version" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 5, 1, 11, 30, 0, 0, DateTimeKind.Utc), "valakhanovich", "Prod", 1, "https://logs.example.com/cicd/orchestrator/deploy/1.0.0", "Success", "1.0.0" },
                    { 2, new DateTime(2025, 5, 2, 7, 15, 0, 0, DateTimeKind.Utc), "kirilov", "Prod", 2, "https://logs.example.com/monitoring/collector/deploy/1.0.0", "Success", "1.0.0" },
                    { 3, new DateTime(2025, 5, 2, 7, 20, 0, 0, DateTimeKind.Utc), "kirilov", "Prod", 3, "https://logs.example.com/monitoring/notifier/deploy/1.0.0", "Success", "1.0.0" },
                    { 4, new DateTime(2025, 4, 28, 14, 0, 0, 0, DateTimeKind.Utc), "dronov", "Staging", 4, "https://logs.example.com/apigw/core/deploy/2.1.0", "Success", "2.1.0" },
                    { 5, new DateTime(2025, 5, 5, 6, 45, 0, 0, DateTimeKind.Utc), "ivanchenko", "Prod", 5, "https://logs.example.com/user/api/deploy/1.0.1", "Success", "1.0.1" },
                    { 6, new DateTime(2025, 5, 7, 10, 0, 0, 0, DateTimeKind.Utc), "sergeev", "Dev", 6, "https://logs.example.com/billing/core/deploy/3.0.0-beta", "Success", "3.0.0-beta" },
                    { 7, new DateTime(2025, 5, 6, 8, 30, 0, 0, DateTimeKind.Utc), "stepanova", "Staging", 7, "https://logs.example.com/notify/sender/deploy/1.2.0", "Failed", "1.2.0" },
                    { 8, new DateTime(2025, 5, 3, 12, 10, 0, 0, DateTimeKind.Utc), "nikitin", "Prod", 8, "https://logs.example.com/analytics/ingest/deploy/2.0.0", "Success", "2.0.0" },
                    { 9, new DateTime(2025, 5, 4, 15, 0, 0, 0, DateTimeKind.Utc), "andreeva", "Prod", 9, "https://logs.example.com/auth/core/deploy/1.3.0", "Success", "1.3.0" },
                    { 10, new DateTime(2025, 5, 8, 9, 0, 0, 0, DateTimeKind.Utc), "kozlova", "Staging", 10, "https://logs.example.com/portal/frontend/deploy/1.0.0-rc", "Success", "1.0.0-rc" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Deployments_MicroserviceId",
                table: "Deployments",
                column: "MicroserviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Microservices_ProjectId",
                table: "Microservices",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_OwnerId",
                table: "Projects",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Deployments");

            migrationBuilder.DropTable(
                name: "Microservices");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
