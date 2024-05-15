using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI.Chat.Copilot.Models
{
    public class CreateEditAppArgs : EventArgs
    {
        public UserControl UserControl { get; set; }
    }
}
