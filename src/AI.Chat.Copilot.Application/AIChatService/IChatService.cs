using AI.Chat.Copilot.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI.Chat.Copilot.Application.AIChatService
{
    public interface  IChatService
    {
        IAsyncEnumerable<string> SendStreamAsync(AIApps app,string content,IEnumerable<AppChatMessage> history,CancellationToken cancellationToken=default);
    }
}
