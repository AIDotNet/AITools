using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI.Chat.Copilot.ViewModels
{
    internal class ModelDownloadTipViewModel : ViewModelBase
    {
        public static string mdTemplate = """
            ### 1、安装魔塔所需环境
            ```
            pip install modelscope -i https://mirrors.aliyun.com/pypi/simple/
            ```

            ### 2、py 脚本
            ```
            from modelscope.hub.snapshot_download import snapshot_download
            snapshot_download(f'{0}', cache_dir='存储路径')
            ```
            
            """;
        private string _mdtext;
        public string MdText
        {
            get => this._mdtext;
            set => this.RaiseAndSetIfChanged(ref this._mdtext, value);
        }
    }
}
