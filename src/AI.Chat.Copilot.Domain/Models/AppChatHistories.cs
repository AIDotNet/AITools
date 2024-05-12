using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI.Chat.Copilot.Domain.Models
{
    public class AppChatHistories
    {
        public int Id { get; set; }
        public int ChatId { get; set; }
        public required string RoleName { get; set; }
        public required string Content { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
