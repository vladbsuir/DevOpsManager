namespace DevOpsManager.Models
{
    [Flags]
    public enum AccessPermissions
    {
        None = 0,

        Users = 1,                            // доступ к управлению пользователями

        Projects = 1 << 1,                   // управление проектами

        MicroservicesRead = 1 << 2,         // просмотр микросервисов
        MicroservicesEdit = 1 << 3,         // редактирование микросервисов

        DeployRead = 1 << 4,                // просмотр деплоев
        DeployEdit = 1 << 5                 // возможность запускать деплой
    }
}
