using AI.Chat.Copilot.Application;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using AI.Chat.Copilot.Infrastructure;
namespace AI.Chat.Copilot.Test
{
    public abstract class SemanticKernelBaseTest
    {
        protected IKernelBuilder _builder;
        protected ITestOutputHelper _testOutputHelper;
        protected IServiceProvider _serviceProvider;
        protected SemanticKernelBaseTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _builder = Microsoft.SemanticKernel.Kernel.CreateBuilder();
            var services = new ServiceCollection();
            services.AddApplicationService();
            services.AddEFCoreRepository();
            _serviceProvider = services.BuildServiceProvider();
        }

        protected Kernel Kernel => _builder.Build();
    }
}
