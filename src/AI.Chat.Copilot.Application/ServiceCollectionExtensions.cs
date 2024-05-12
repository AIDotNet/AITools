using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI.Chat.Copilot.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services.AddTransient<AIApplicationAppService>();
            return services;
        }
    }
}
