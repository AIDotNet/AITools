using AI.Chat.Copilot.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI.Chat.Copilot.Infrastructure
{
    public class AIToolDbContext : DbContext
    {
        public DbSet<AIApps> AIApps { get; set; }
        public DbSet<AppChat> AppChats { get; set; }
        public DbSet<AppChatHistories> AppChatHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AIToolDbContext).Assembly);
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(connectionString: "Host=localhost;Username=root;Password=1;Database=ai-tools",
                builder => { builder.EnableRetryOnFailure(); });
            optionsBuilder.LogTo(async msg => {
                await File.AppendAllTextAsync(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ef.log"), msg, Encoding.UTF8);
            }, LogLevel.Information).EnableSensitiveDataLogging();
        }

    }
}
