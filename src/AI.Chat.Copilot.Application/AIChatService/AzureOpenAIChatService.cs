using AI.Chat.Copilot.Domain.Models;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI.Chat.Copilot.Application.AIChatService
{
    public class AzureOpenAIChatService : IChatService
    {
        public async IAsyncEnumerable<string> SendStreamAsync(AIApps app, string content, IEnumerable<AppChatMessage> history, CancellationToken cancellationToken = default)
        {
            var kernel = Kernel.CreateBuilder()
                .AddAzureOpenAIChatCompletion(app.ModelId,app.Endpoint,app.Secret)
                .Build();
            var chat = kernel.GetRequiredService<IChatCompletionService>();
            var settings = new OpenAIPromptExecutionSettings
            {
                MaxTokens = app.MaxTokens,
                Temperature = app.Temperature / 100
            };
            await foreach (var item in chat.GetStreamingChatMessageContentsAsync(new ChatHistory(history.Select(u => new ChatMessageContent(ConvertRole(u.Role), content: u.Content))), settings, kernel, cancellationToken))
            {
                if (item.Content is not null)
                {
                    yield return item.Content.ToString();
                }
            }
        }

        private AuthorRole ConvertRole(string role)
        {
            switch (role.ToUpper())
            {
                case "Assistant":
                    return AuthorRole.Assistant;
                case "user":
                    return AuthorRole.User;
                case "system":
                    return AuthorRole.System;
                case "tool":
                    return AuthorRole.Tool;
                default:
                    return AuthorRole.System;
            }
        }
    }
}
