using System.ComponentModel.DataAnnotations.Schema;

namespace DevOpsManager.Models
{
    // Таблица пользователей
    public class User
    {
        public int Id { get; set; } // Уникальный идентификатор пользователя
        public string Username { get; set; } // Имя пользователя
        public string PasswordHash { get; set; } // Хэш пароля
        public string Email { get; set; } // Электронная почта
        public int RoleId { get; set; } // Внешний ключ на таблицу ролей
        public DateTime CreatedAt { get; set; } // Дата регистрации

        public DateTime? LastLoginAt { get; set; }

        // Навигационное свойство для роли
        public Role Role { get; set; }

        // Навигационное свойство для проектов, созданных пользователем
        public ICollection<Project> Projects { get; set; }
    }
}
