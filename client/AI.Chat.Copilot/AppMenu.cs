using Avalonia.Controls;
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
    public record AppMenu(string Title,string Icon, Type UseControlType)
    {
        public static AppMenu Index() => new AppMenu("首页", "HomeCircle", typeof(Index));

        public static AppMenu Chat() => new AppMenu("会话", "MessageOutline", typeof(ChatList));

        public static AppMenu App() => new AppMenu("应用", "GamepadCircleOutline", typeof(Applications));

        public static AppMenu GlobalSettings() => new AppMenu("设置", "CogOutline", typeof(GlobalSettings));
    }
}
