using AI.Chat.Copilot.Application;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace AI.Chat.Copilot.Test
{
    public class OpenAITest : SemanticKernelBaseTest
    {

        public OpenAITest(ITestOutputHelper testOutputHelper) :base(testOutputHelper)
        {
           var config =  _serviceProvider.GetRequiredService<AIApplicationAppService>().QueryAsync("").GetAwaiter().GetResult().FirstOrDefault(u=>u.AIModelType == Domain.Shared.AIModelType.OpenAI);
            _builder.AddOpenAIChatCompletion(
                modelId: config?.ModelId ?? "", 
                apiKey: config?.Secret ?? "",
                httpClient: new HttpClient(new OpenAIHttpClientHandler(config?.ProxyHost ?? string.Empty)));
        }

        [Fact]
        public async Task RunAsync()
        {
            _testOutputHelper.WriteLine(JsonConvert.SerializeObject(await Kernel.InvokePromptAsync("What color is the sky?")));
            

            // Example 2. Invoke the kernel with a templated prompt and display the result
            KernelArguments arguments = new() { { "topic", "sea" } };
            _testOutputHelper.WriteLine(JsonConvert.SerializeObject(await Kernel.InvokePromptAsync("What color is the {{$topic}}?", arguments)));
           

            // Example 3. Invoke the kernel with a templated prompt and stream the results to the display
            await foreach (var update in Kernel.InvokePromptStreamingAsync("What color is the {{$topic}}? Provide a detailed explanation.", arguments))
            {
                _testOutputHelper.WriteLine(JsonConvert.SerializeObject(update));
            }

           
            // Example 4. Invoke the kernel with a templated prompt and execution settings
            arguments = new(new OpenAIPromptExecutionSettings { MaxTokens = 500, Temperature = 0.5 }) { { "topic", "dogs" } };
            _testOutputHelper.WriteLine(JsonConvert.SerializeObject(await Kernel.InvokePromptAsync("Tell me a story about {{$topic}}", arguments)));

            // Example 5. Invoke the kernel with a templated prompt and execution settings configured to return JSON
#pragma warning disable SKEXP0010
            arguments = new(new OpenAIPromptExecutionSettings { ResponseFormat = "json_object" }) { { "topic", "chocolate" } };
            _testOutputHelper.WriteLine(JsonConvert.SerializeObject(await Kernel.InvokePromptAsync("Create a recipe for a {{$topic}} cake in JSON format", arguments)));
        }
    }
}
