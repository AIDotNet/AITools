using Avalonia.Controls;
using Material.Icons.Avalonia;
using Material.Icons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI.Chat.Copilot
{
    /// <summary>
    /// 菜单
    /// </summary>
    /// <param name="title"></param>
    /// <param name="icon"></param>
    /// <param name="useControlType"></param>
    public static class AppMenu
    {
        public static SideMenuItem Index() => new SideMenuItem
        {
            PageContent = typeof(Index),
            Header = "首页",
            Icon = new MaterialIcon { Kind = MaterialIconKind.HomeCircle,Width = 25,Height=25 },
        };
        public static SideMenuItem Chat() => new SideMenuItem
        {
            PageContent = typeof(Chat),
            Header = "会话",
            Icon = new MaterialIcon { Kind = MaterialIconKind.MessageOutline, Width = 25, Height = 25 },
        };
        public static SideMenuItem App() => new SideMenuItem
        {
            PageContent = typeof(Applications),
            Header = "应用",
            Icon = new MaterialIcon { Kind = MaterialIconKind.GamepadCircleOutline, Width = 25, Height = 25 }
        };
        public static SideMenuItem GlobalSettings() => new SideMenuItem
        {
            PageContent = typeof(GlobalSettings),
            Header = "设置",
            Icon = new MaterialIcon { Kind = MaterialIconKind.CogOutline, Width = 25, Height = 25 }
        };
    }
}
