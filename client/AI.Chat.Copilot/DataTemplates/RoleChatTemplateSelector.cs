using AI.Chat.Copilot.Models;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AI.Chat.Copilot.DataTemplates
{
    public class RoleChatTemplateSelector : IDataTemplate
    {
        [Content]
        public Dictionary<string, IDataTemplate> AvailableTemplates { get; } = new Dictionary<string, IDataTemplate>();
        public Control? Build(object? param)
        {
            string? key =  null;

            if (param is ChatHistory chat)
            {
                key = chat.Role;
            }
            if (key is null)
            {
                throw new ArgumentNullException(nameof(param));
            }
            return AvailableTemplates[key].Build(param); 
        }

        public bool Match(object? data)
        {
            if(data is ChatHistory chat)
            {
                return  !string.IsNullOrEmpty(chat.Role)           
                       && AvailableTemplates.ContainsKey(chat.Role);
            }
            return false;
        }
    }
}
