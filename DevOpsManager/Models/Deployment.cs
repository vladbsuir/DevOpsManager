namespace DevOpsManager.Models
{

    public class Deployment
    {
        public int Id { get; set; }                         // Уникальный идентификатор
        public int MicroserviceId { get; set; }             // Внешний ключ на микросервис
        public string Version { get; set; }                 // Версия сборки
        public DateTime DeployedAt { get; set; }            // Дата и время развертывания
        public string Environment { get; set; }             // Среда (prod, dev, staging и т.д.)
        public string DeployedBy { get; set; }              // Имя или логин пользователя
        public string Status { get; set; }                  // Статус (успешно, с ошибкой и т.д.)
        public string RemoteLogsUrl { get; set; }           // Ссылка на удалённые логи

        public Microservice Microservice { get; set; }      // Навигационное свойство
    }
}

