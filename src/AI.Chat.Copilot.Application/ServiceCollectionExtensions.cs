using AI.Chat.Copilot.Application.AIChatService;
using AI.Chat.Copilot.Domain.Shared;
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
            services.AddTransient<AppChatService>();
            services.AddKeyedScoped<IChatService, OpenAIChatService>(AIModelType.OpenAI);
            return services;
        }
    }
}
