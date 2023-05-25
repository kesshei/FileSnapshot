using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Model   
{
    public class SnapshotManage : ISnapshotManage
    {
        public bool Delete(string id)
        {
            throw new NotImplementedException();
        }

        public bool Delete(IFileSnapshot fileSnapshot)
        {
            throw new NotImplementedException();
        }

        public List<IFileSnapshot> GetFileSnapshots(string versionid = null)
        {
            throw new NotImplementedException();
        }

        public void Switch(string versionid)
        {
            throw new NotImplementedException();
        }

        public void Switch(IFileSnapshot fileSnapshot)
        {
            throw new NotImplementedException();
        }
    }
}
