namespace DevOpsManager.Models
{
    public class Project
    {
        public int Id { get; set; }                           // Уникальный идентификатор проекта
        public string Name { get; set; }                      // Название проекта
        public string Code { get; set; }                      // Уникальный короткий код проекта (например: "PRJ-001")
        public string Description { get; set; }               // Описание проекта
        public string JiraUrl { get; set; }                   // Ссылка на систему управления задачами (например: Jira)
        public string ConfluenceUrl { get; set; }             // Ссылка на базу знаний (например: Confluence)
        public string GitRepository { get; set; }             // Ссылка на репозиторий кода (например: GitLab или GitHub)
        public string SlackChannel { get; set; }              // Ссылка или имя канала в Slack
        public DateTime StartDate { get; set; }               // Дата начала проекта
        public DateTime? EndDate { get; set; }                // Дата завершения (если завершён)
        public string Status { get; set; }                    // Статус проекта (Активен, Приостановлен, Завершён и т.п.)
        public string Environment { get; set; }               // Окружение (Dev, Staging, Prod)
        public string DeploymentUrl { get; set; }             // URL сервера деплоя или CI/CD dashboard
        public int OwnerId { get; set; }                      // Ответственный пользователь (внешний ключ на User)

        // Навигационные свойства
        public User Owner { get; set; }                       // Владелец проекта
        public ICollection<Microservice> Microservices { get; set; } // Связанные микросервисы
    }
}
