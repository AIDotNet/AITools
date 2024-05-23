using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.EFCore.GenericRepository;

namespace AI.Chat.Copilot.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEFCoreRepository(this IServiceCollection services)
        {
            services.AddDbContext<AIToolDbContext>();
            services.AddGenericRepository<AIToolDbContext>();
            services.AddQueryRepository<AIToolDbContext>();
            return services;
        }
    }
}
