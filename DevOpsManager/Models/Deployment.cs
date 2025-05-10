namespace DevOpsManager.Models
{
    // Таблица развертываний микросервисов
    public class Deployment
    {
        public int Id { get; set; } // Уникальный идентификатор развертывания
        public int MicroserviceId { get; set; } // Внешний ключ на микросервис
        public DateTime Timestamp { get; set; } // Время развертывания
        public string Status { get; set; } // Статус (успешно, ошибка и т.д.)
        public string Log { get; set; } // Текст логов развертывания

        // Навигационное свойство микросервиса
        public Microservice Microservice { get; set; }
    }
}

