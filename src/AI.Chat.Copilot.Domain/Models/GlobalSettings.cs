using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI.Chat.Copilot.Domain.Models
{
    [AddINotifyPropertyChangedInterface]
    public class GlobalSettings
    {
        public int Id { get; set; }
        public float MyProperty { get; set; }
    }
}
