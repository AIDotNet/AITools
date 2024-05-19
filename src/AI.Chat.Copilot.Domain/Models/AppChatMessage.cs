using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI.Chat.Copilot.Domain.Models
{
    [AddINotifyPropertyChangedInterface]
    public class AppChatMessage
    {
        public int Id { get; set; }
        public string ChatId { get; set; }
        public  string Role { get; set; }
        public  string Content { get; set; }
        public DateTime CreateTime { get; set; }
        [NotMapped]
        public bool IsWriting { get; set; }
    }
}
