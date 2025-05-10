namespace DevOpsManager.Models
{
    // Таблица проектов
    public class Project
    {
        public int Id { get; set; } // Уникальный идентификатор проекта
        public string Name { get; set; } // Название проекта
        public string RepoUrl { get; set; } // Ссылка на репозиторий
        public string Description { get; set; } // Описание проекта
        public int CreatedBy { get; set; } // ID пользователя, создавшего проект
        public DateTime CreatedAt { get; set; } // Дата создания проекта

        // Ссылки на внешние инструменты мониторинга и визуализации
        public string GrafanaDashboardUrl { get; set; } // Ссылка на дашборд Grafana
        public string PrometheusUrl { get; set; } // Ссылка на метрики Prometheus

        // Навигационное свойство для пользователя
        public User Creator { get; set; }

        // Список микросервисов, связанных с этим проектом
        public ICollection<Microservice> Microservices { get; set; }
    }
}
