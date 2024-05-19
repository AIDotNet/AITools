using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI.Chat.Copilot.Test
{
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
                    Host = _proxy.Replace("http://","").Replace("https://","").Replace("/",""),
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
