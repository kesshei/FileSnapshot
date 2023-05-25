using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Model   
{
    public class Snapshot : IFileSnapshot
    {
        public string VersionID { get; set; }
        public long Timestamp { get; set; }
        public List<FileInformation> Files { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsRead { get; set; }

        public DateTime GetDateTime()
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(Timestamp).LocalDateTime;
        }
        public long GenerateTimestamp()
        {
            return new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        }
    }
}
