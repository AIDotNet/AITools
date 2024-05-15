using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI.Chat.Copilot.Domain.Shared
{
    public enum AIModelType
    {
        OpenAI = 0x01,
        AzureOpenAI = 0x02,
        LLamaSharp = 0x03,
        ChatGLM = 0x04,
    }

    public static class AIModelManager
    {
        public static List<AIModelType> AIModelTypes = new List<AIModelType> { 
            AIModelType.OpenAI, 
            AIModelType.AzureOpenAI, 
            AIModelType.LLamaSharp, 
            AIModelType.ChatGLM 
        };
        public static List<string> AIModels = [ "gpt-3.5-turbo",
        "gpt-3.5-turbo-0125",
        "gpt-3.5-turbo-0301",
        "gpt-3.5-turbo-0613",
        "gpt-3.5-turbo-1106",
        "gpt-3.5-turbo-16k",
        "gpt-3.5-turbo-16k-0613"];   
    }
}
