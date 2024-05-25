using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace AI.Chat.Copilot.Application
{
    public class OpenAITokenRecordQueue
    {
        private Channel<OpenAIToken> _channel;
        public OpenAITokenRecordQueue()
        {
            _channel = Channel.CreateBounded<OpenAIToken>(new BoundedChannelOptions(1000)
            {
                AllowSynchronousContinuations = true,
                SingleReader = true,
                SingleWriter = true,
            });
        }
        public ValueTask EnqueueAsync(OpenAIToken openAIToken)
        {
            return _channel.Writer.WriteAsync(openAIToken);
        }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                while (await _channel.Reader.WaitToReadAsync())
                {
                    if (_channel.Reader.TryRead(out var item))
                    {
                        Console.WriteLine($"消费数据：{JsonSerializer.Serialize(item)}");
                    }
                }
            }
        }
    }

    public record OpenAIToken (int AppId,int CompletionTokenCount,int PromptTokenCount,double Duration);
}
