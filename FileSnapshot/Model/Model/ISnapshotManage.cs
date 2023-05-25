using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Model   
{
    public interface ISnapshotManage
    {
        List<IFileSnapshot> GetFileSnapshots(string versionid = null);
        bool Delete(string id);
        bool Delete(IFileSnapshot fileSnapshot);
        void Switch(string versionid);
        void Switch(IFileSnapshot fileSnapshot);
    }
}
