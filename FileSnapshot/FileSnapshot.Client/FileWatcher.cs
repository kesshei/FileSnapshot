using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSnapshot.Client
{
    public class FileWatcher : IDisposable
    {
        FileSystemWatcher watcher;
        string Path { get; set; }
        string Filter = "*.*";
        NotifyFilters NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.Size;
        bool IncludeSubdirectories = true;
        bool IsRun = false;
        private Queue<string> _queue;
        public FileWatcher(string path)
        {

            this.Path = path;
            watcher = new FileSystemWatcher();
            watcher.Path = Path;
            watcher.Filter = this.Filter;
            watcher.NotifyFilter = this.NotifyFilter;
            watcher.IncludeSubdirectories = this.IncludeSubdirectories;
            _queue = new Queue<string>();
        }
        /// <summary>
        /// 开始监控
        /// </summary>
        public void StartWatcher()
        {
            if (watcher != null)
            {
                watcher.EnableRaisingEvents = true;
                watcher.Created += new FileSystemEventHandler(OnProcess);
                watcher.Changed += new FileSystemEventHandler(OnProcess);
                watcher.Deleted += new FileSystemEventHandler(OnProcess);
                watcher.Renamed += new RenamedEventHandler(OnProcess);
                IsRun = true;
            }
        }
        /// <summary>
        /// 停止监控
        /// </summary>
        public void CloseWatcher()
        {
            if (watcher != null)
            {
                watcher.EnableRaisingEvents = false;
                watcher.Created -= new FileSystemEventHandler(OnProcess);
                watcher.Changed -= new FileSystemEventHandler(OnProcess);
                watcher.Deleted -= new FileSystemEventHandler(OnProcess);
                watcher.Renamed -= new RenamedEventHandler(OnProcess);
            }
            IsRun = false;
        }
        /// <summary>
        /// 当创建新文件的时候
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnProcess(object source, FileSystemEventArgs e)
        {
            if (!IsRun)
            {
                return;
            }
            _queue.Enqueue(e.FullPath);
        }
        public bool IsChange()
        {
            return _queue.Count > 0;
        }
        public void ClearChange()
        {
            _queue.Clear();
        }

        public void Dispose()
        {
            CloseWatcher();
        }
    }
}
