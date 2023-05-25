
using Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSnapshot.Server
{
    public class FileSnapshotServer : IFileSnapshotServer
    {
        public string BaseFilePath { get; set;}
        public ISnapshotManage snapshotManage { get; set; }

        public string GetFilePath(string hash)
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
