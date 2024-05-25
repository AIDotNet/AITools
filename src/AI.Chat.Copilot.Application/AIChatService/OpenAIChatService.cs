using AI.Chat.Copilot.Domain.Models;
using Azure.AI.OpenAI;
using Microsoft.Extensions.Azure;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.Plugins.Core;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AI.Chat.Copilot.Application.AIChatService
{
    public class OpenAIChatService : AIChatBaseService, IChatService
    {

      private readonly static string prompt = """
            ### 当前会话中你自身的设定{{$tip}}
            ### 历史会话
            {{$history}}
            #### 用户问题
            User: {{$input}}
            #### 回答要求
            1. 如果你知道答案，请详细解答。
            2. 如果你不知道答案，请直接回复“我不知道”。
            请不要编造或猜测答案。谢谢！
            #### AI 回答
            Assistant:
            """;

        private static ConcurrentDictionary<string, HttpClient> _clients = new ConcurrentDictionary<string, HttpClient>();
        private OpenAITokenRecordQueue Queue { get; }
        public OpenAIChatService(OpenAITokenRecordQueue queue)
        {
            Queue = queue;
        }
        protected override void AddChatComplateService(AIApps app, IKernelBuilder builder)
        {
            HttpClient? httpClient = string.IsNullOrWhiteSpace(app.Endpoint) ? null : _clients.GetOrAdd(app.Endpoint, key =>
            {
                return new HttpClient(new OpenAIHttpClientHandler(key));
            });
            builder.AddOpenAIChatCompletion(modelId: app.ModelId!, apiKey: app.Secret!, httpClient: httpClient);
        }
        public async IAsyncEnumerable<string> SendStreamAsync(AIApps app, string content, IEnumerable<AppChatMessage> history, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var kernel = CreateChatKernelByApp(app);
            var settings = new OpenAIPromptExecutionSettings
            {
                MaxTokens = app.MaxTokens,
                Temperature = app.Temperature / 100
            };
            KernelFunction func;
            string historyMsg = string.Join("\n", history.Select(x => x.Role + ": " + x.Content));
            if (history.Count() >= 5)
            {
                func = kernel.Plugins.GetFunction("ConversationSummaryPlugin", "SummarizeConversation");
                string msg = $"用中文总结以下内容：{historyMsg}，要求简洁明了，字数控制在200字以内。";
                Stopwatch stopwatch = Stopwatch.StartNew();
                func = kernel.CreateFunctionFromPrompt("{{ConversationSummaryPlugin.SummarizeConversation $input}}", new OpenAIPromptExecutionSettings
                {
                    ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
                });
                var result = await kernel.InvokeAsync(func, new KernelArguments
                {
                    ["input"] = msg
                }, cancellationToken);
                historyMsg = result.GetValue<string>() ?? historyMsg;
                stopwatch.Stop();
                if (result.Metadata?.ContainsKey("Usage") ?? false)
                {
                    var usage = (CompletionsUsage)result.Metadata["Usage"];
                    await Queue.EnqueueAsync(new OpenAIToken(app.Id, usage.CompletionTokens, usage.PromptTokens, stopwatch.ElapsedMilliseconds));
                }
            }
            var promptTemplate = new KernelPromptTemplateFactory().Create(new PromptTemplateConfig(prompt)
            {
                ExecutionSettings = new Dictionary<string, PromptExecutionSettings>
                {
                    { "default", settings }
                }
            });
            func = KernelFunctionFactory.CreateFromPrompt(prompt, settings);
            string appPrompt = app.Prompt ?? "你是一个AI智能人工助手";

            int count = 0;
            var arg = new KernelArguments
            {
                ["tip"] = appPrompt,
                ["input"] = content.Trim(),
                ["history"] = historyMsg
            };
            Stopwatch sw = Stopwatch.StartNew();
            await foreach (var item in kernel.InvokeStreamingAsync(func, arg , cancellationToken))
            {
                if (item is not null)
                {
                    count += Tokenizers.GetTokens(app.ModelId!, item.ToString());
                    yield return item.ToString();
                }
            }
            sw.Stop();
            string asw = await promptTemplate.RenderAsync(kernel, arg);
            await Queue.EnqueueAsync(new OpenAIToken(app.Id, Tokenizers.GetTokens(app.ModelId, asw), count, sw.ElapsedMilliseconds));
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
    
    public class OpenAIHttpClientHandler : HttpClientHandler
    {
        private string _proxy;
        public OpenAIHttpClientHandler(string proxy) : base()
        {
            _proxy = proxy;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(_proxy))
            {
                UriBuilder uriBuilder = new UriBuilder(request.RequestUri!)
                {
                    // 这里是你要修改的 URL
                    Scheme = _proxy,
                    Host = _proxy.Replace("http://", "").Replace("https://", "").Replace("/", ""),
                    Path = request.RequestUri.AbsolutePath,
                };
                request.RequestUri = uriBuilder.Uri;
            }
            
            // 接着，调用基类的 SendAsync 方法将你的修改后的请求发出去
            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            return response;
        }
    }
}
