using AI.Chat.Copilot.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Plugins.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AI.Chat.Copilot.Application.AIChatService
{
    public abstract class AIChatBaseService 
    {
        protected Kernel CreateChatKernelByApp(AIApps app)
        {
            var builder = Kernel.CreateBuilder();
            builder.Services.AddLogging(config =>
            {
                config.SetMinimumLevel(LogLevel.Trace);
                config.AddConsole();
            });
            AddChatComplateService(app, builder);
#pragma warning disable SKEXP0050 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            builder.Plugins.AddFromType<ConversationSummaryPlugin>()
                           .AddFromType<TimePlugin>();
#pragma warning restore SKEXP0050 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

            return builder.Build();
        }
        protected abstract void AddChatComplateService(AIApps app,IKernelBuilder builder);
    }
}
