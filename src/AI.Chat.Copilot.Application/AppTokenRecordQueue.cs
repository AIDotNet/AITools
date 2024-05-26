using Lucene.Net.Documents;
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
                        LuceneService.WriteDoc(AppConst.OpenAITokenIndex, OpenAIToken.Doc(item));
                    }
                }
            }
        }
    }

    public record OpenAIToken(int AppId, string AppName,int CompletionTokenCount, int PromptTokenCount, double Duration,string DateTime)
    {
        public static Document Doc(OpenAIToken data)
        {
            var doc = new Document();
            doc.Add(new Int32Field("AppId", data.AppId, Field.Store.YES));
            doc.Add(new StringField("AppName", data.AppName, Field.Store.YES));
            doc.Add(new Int32Field("CompletionTokenCount", data.CompletionTokenCount, Field.Store.YES));
            doc.Add(new Int32Field("PromptTokenCount", data.PromptTokenCount, Field.Store.YES));
            doc.Add(new DoubleField("Duration", data.Duration, Field.Store.YES));
            doc.Add(new StringField("CreateDate", data.DateTime, Field.Store.YES));
            return doc;
        }
        public static OpenAIToken Get(Document doc)
        {
            return new OpenAIToken(doc.GetField("AppId").GetInt32Value().Value, doc.Get("AppName"), doc.GetField("CompletionTokenCount").GetInt32Value().Value, doc.GetField("PromptTokenCount").GetInt32Value().Value, doc.GetField("Duration").GetDoubleValue().Value, doc.Get("CreateDate"));
        }
    }

}
