using Microsoft.Extensions.Configuration;
using Python.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI.Chat.Copilot.Application.HuggingFaceService
{
    /// <summary>
    /// 模型下载
    /// </summary>
    public sealed class ModelDownloadService
    {
        private IConfigurationRoot Configuration { get; }
        public ModelDownloadService(IConfigurationRoot configuration)
        {
            Configuration = configuration;
        }

        //public IAsyncEnumerable<string> DownloadAsync(string modelName, string savePath)
        //{
        //    if (string.IsNullOrWhiteSpace(Runtime.PythonDLL))
        //    {
        //        Runtime.PythonDLL = Configuration["PyPath"];
        //    }
        //    PythonEngine.Initialize();
        //}
    }
}
