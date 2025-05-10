namespace DevOpsManager.Models
{
    // Таблица ролей пользователей (например, Admin, DevOps, Viewer)
    public class Role
    {
        public int Id { get; set; } // Уникальный идентификатор роли
        public string Name { get; set; } // Название роли
        public AccessPermissions Permissions { get; set; }
        // Список пользователей с этой ролью
        public ICollection<User> Users { get; set; }
    }
}
