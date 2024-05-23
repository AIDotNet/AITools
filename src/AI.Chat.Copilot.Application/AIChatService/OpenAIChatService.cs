using AI.Chat.Copilot.Domain.Models;
using Microsoft.Extensions.Azure;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AI.Chat.Copilot.Application.AIChatService
{
    public class OpenAIChatService : IChatService
    {
        private static ConcurrentDictionary<string, HttpClient> _clients = new ConcurrentDictionary<string, HttpClient>();

        public async IAsyncEnumerable<string> SendStreamAsync(AIApps app,string content ,IEnumerable<AppChatMessage> history, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            HttpClient? httpClient = string.IsNullOrWhiteSpace(app.ProxyHost) ? null : _clients.GetOrAdd(app.ProxyHost, key => {
                return new HttpClient(new OpenAIHttpClientHandler(key));
            });
            var kernel = Kernel.CreateBuilder()
                .AddOpenAIChatCompletion(modelId: app.ModelId, apiKey: app.Secret, httpClient: httpClient)
                .Build();
            var func = kernel.CreateFunctionFromPrompt(string.IsNullOrEmpty(app.Prompt) ? "{{$input}}" : app.Prompt, new OpenAIPromptExecutionSettings
            {
                MaxTokens = app.MaxTokens,
                Temperature = app.Temperature / 100
            });
            await foreach (var item in kernel.InvokeStreamingAsync(func,new KernelArguments { 
                ["input"] = content
                },cancellationToken))
            {
                yield return item.ToString();
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
