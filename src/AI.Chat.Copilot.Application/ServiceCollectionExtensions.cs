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
            services.AddKeyedScoped<IChatService, OpenAIChatService>(AIModelType.AzureOpenAI);
            services.AddSingleton<OpenAITokenRecordQueue>();
            services.AddTransient<HFMirrorService>();
            services.AddHttpClient(HFMirrorService.Client, client =>
            {
                client.BaseAddress = new Uri("https://hf-mirror.com");
                client.DefaultRequestHeaders.UserAgent.Clear();
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/123.0.0.0 Safari/537.36");
            });
            return services;
        }
    }
}
