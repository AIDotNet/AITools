using AI.Chat.Copilot.Application;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace AI.Chat.Copilot.Test
{
    public class HFMirrorTest : SemanticKernelBaseTest
    {
        public HFMirrorTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public async Task Should_Be_True()
        {
           var service =  _serviceProvider.GetRequiredService<HFMirrorService>();
           var result =  await service.GetListAsync("qwen", 0);
           Assert.NotNull(result);
            _testOutputHelper.WriteLine(JsonSerializer.Serialize(result));
        }
    }
}
