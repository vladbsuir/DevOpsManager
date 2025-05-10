namespace DevOpsManager.Models
{
    // Таблица микросервисов
    public class Microservice
    {
        public int Id { get; set; } // Уникальный идентификатор микросервиса
        public string Name { get; set; } // Название микросервиса
        public string ImageUrl { get; set; } // Ссылка на образ контейнера
        public string Environment { get; set; } // Среда развертывания (dev, staging, prod и т.д.)
        public int ProjectId { get; set; } // Внешний ключ на проект

        // Ссылки на связанные сервисы
        public string GrafanaPanelUrl { get; set; } // Ссылка на панель мониторинга в Grafana
        public string LogsUrl { get; set; } // Ссылка на логи микросервиса
        public string HealthcheckEndpoint { get; set; } // Ссылка на healthcheck-эндпоинт микросервиса

        // Навигационное свойство проекта
        public Project Project { get; set; }

        // Список развертываний микросервиса
        public ICollection<Deployment> Deployments { get; set; }
    }
}
