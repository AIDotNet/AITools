using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI.Chat.Copilot.Models
{
    public class AppChatDto
    {
        public int Id { get; set; }
        public int AppId { get; set; }
        public string Title { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
