from modelscope.hub.snapshot_download import snapshot_download
import time
import os
import argparse
import sys
import signal

try:
    import modelscope
except ImportError:
    print("pip install modelscope -i https://mirrors.aliyun.com/pypi/simple/")
    os.system("pip install modelscope -i https://mirrors.aliyun.com/pypi/simple/")


parser = argparse.ArgumentParser(description="魔塔下载模型脚本")
parser.add_argument(
    "--model",
    "-M",
    default=None,
    type=str,
    help="模型名称",
)
parser.add_argument(
    "--save-dir",
    "-S",
    default=None,
    type=str,
    help="文件夹路径",
)

args = parser.parse_args()

if args.model is None :
    print(
        "Specify the name of the model, e.g., --model baichuan-inc/Baichuan2-7B-Chat"
    )
    sys.exit()

if args.save_dir is None :
    print(
        "Specify the name of the save_dir, e.g., --save_dir /models"
    )
    sys.exit()



def download_snapshot(model_id):
    print('Downloading snapshot {}'.format(model_id))
    snapshot_download(f'{model_id}', cache_dir=args.save_dir)

def signal_handler(signal, frame):
    print("\nProcess interrupted! Exiting gracefully...")
    sys.exit(0)

def main():
    # Register the signal handler for KeyboardInterrupt (Ctrl+C)
    signal.signal(signal.SIGINT, signal_handler)
    start = time.time()
    download_snapshot(args.model)
    end = time.time()
    print('Total elapsed time: {}'.format(end - start))

if __name__ == '__main__':
    main()
