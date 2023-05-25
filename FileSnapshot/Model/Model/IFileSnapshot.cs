using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Model.Model   
{
    public interface IFileSnapshot : IFileAuthor
    {
        public string VersionID { get; set; }
        public long Timestamp { get; set; }
        public List<FileInformation> Files { get; set; }
        DateTime GetDateTime();
        public bool IsRead { get; set; } 
    }
    public interface IFileAuthor
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
