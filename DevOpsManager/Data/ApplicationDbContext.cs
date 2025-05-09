namespace DevOpsManager.Data
{
    using Microsoft.EntityFrameworkCore;
    using DevOpsManager.Models; // заменить на свой namespace

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        /*public DbSet<Pipeline> Pipelines { get; set; }
        public DbSet<PipelineStep> PipelineSteps { get; set; }
        public DbSet<Deployment> Deployments { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Setting> Settings { get; set; }*/

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }

}
