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
    public class AppChat
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime CreateTime { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedTime { get; set; }
        [NotMapped]
        public bool IsNew { get; set; }
    }
}
