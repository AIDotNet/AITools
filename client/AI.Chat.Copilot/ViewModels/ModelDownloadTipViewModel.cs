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

            ### 2、下载命令行
            ```
            modelscope download --model '{0}' --include '*.json' --cache_dir '存放地址'
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
