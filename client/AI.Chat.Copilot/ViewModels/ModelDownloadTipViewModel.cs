using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AI.Chat.Copilot.ViewModels
{
    internal class ModelDownloadTipViewModel(string mdtext) : ViewModelBase
    {
//         public static string mdTemplate = """
//                                           ### 1、安装魔塔所需环境
//                                           ```
//                                           pip install modelscope -i https://mirrors.aliyun.com/pypi/simple/
//                                           ```
//
//                                           ### 2、py 脚本
//                                           ```
//                                           from modelscope.hub.snapshot_download import snapshot_download
//                                           snapshot_download(f'{0}', cache_dir='{1}')
//                                           ```
//
//                                           """;

        private string _mdtext = mdtext;

        public string MdText
        {
            get => this._mdtext;
            set => this.RaiseAndSetIfChanged(ref this._mdtext, value);
        }


        private ObservableCollection<LogModel> _logs = [];

        /// <summary>
        /// 下载进度打印日志
        /// </summary>
        public ObservableCollection<LogModel> Logs
        {
            get => this._logs;
            set
            {
                if (_logs.Count > 50)
                {
                    _logs.Clear();
                }

                this.RaiseAndSetIfChanged(ref this._logs, value);
            }
        }
    }

    public record LogModel(string Msg)
    {
    }
}