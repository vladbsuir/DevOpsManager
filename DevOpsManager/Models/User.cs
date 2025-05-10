using System.ComponentModel.DataAnnotations.Schema;

namespace DevOpsManager.Models
{    
    public class User
    {
        public int Id { get; set; }                         // Уникальный идентификатор
        public string Username { get; set; }                // Логин
        public string PasswordHash { get; set; }            // Хэш пароля
        public string Email { get; set; }                   // Email
        public int RoleId { get; set; }                     // Роль
        public DateTime CreatedAt { get; set; }             // Дата регистрации
        public DateTime? LastLoginAt { get; set; }          // Последний вход
        public string FullName { get; set; }                // ФИО
        public string SlackHandle { get; set; }             // Ссылка или @ник в Slack
        public string PhoneNumber { get; set; }             // Телефон (внутренний или мобильный)
        public string Position { get; set; }                // Должность (например: Backend Developer)
        public string Department { get; set; }              // Отдел (например: DevOps, Backend, QA)
        public string Location { get; set; }                // Локация (офис, город, удалёнка)
        public DateTime? BirthDate { get; set; }            // Дата рождения (опционально)

        // Навигационные свойства
        public Role Role { get; set; }                      // Роль
        public ICollection<Project> Projects { get; set; } // Связанные проекты
    }
}
