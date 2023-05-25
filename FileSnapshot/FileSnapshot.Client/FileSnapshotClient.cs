
using Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSnapshot.Client
{
    public class FileSnapshotClient : IFileSnapshotClient
    {
        public string ClientID { get; set; }
        public string ServerPath { get; set; }
        public ServerKind ServerKind { get; set; }
        public ISnapshotManage snapshotManage { get; set; }

        public void Start()
        {
        }

        public void Stop()
        {
        }
    }
}
