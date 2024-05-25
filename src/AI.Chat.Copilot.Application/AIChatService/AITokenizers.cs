using AI.Chat.Copilot.Domain.Shared;
using SharpToken;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI.Chat.Copilot.Application.AIChatService
{
    public static class Tokenizers
    {
        private static ConcurrentDictionary<string,GptEncoding> GptEncodings = new ConcurrentDictionary<string,GptEncoding>();

        public static int GetTokens(string model,string content)
        {
           var gptEncoding = GptEncodings.GetOrAdd(model, key =>
            {
                var tokenizers = Model.GetEncodingNameForModel(key);
                return GptEncoding.GetEncoding(tokenizers);
            });
           
            return gptEncoding.Encode(content).Count;
        }
    }
}
