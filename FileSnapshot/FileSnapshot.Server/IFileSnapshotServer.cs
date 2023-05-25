
using Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSnapshot.Server
{
    public interface IFileSnapshotServer
    {
        public string BaseFilePath { get; set; }
        public string GetFilePath(string hash);
        public ISnapshotManage snapshotManage { get; set; }
        public void Start();
        public void Stop();
    }
}
