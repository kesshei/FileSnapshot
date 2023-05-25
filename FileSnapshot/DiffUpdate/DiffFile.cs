using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffUpdate
{
    public class DiffFile
    {
        public string FileName { get; set; }
        public string Path { get; set; }
        public string Hash { get; set; }
        public bool IsDirectory { get; set; }   
        public bool NeedDelete { get; set; }

        // 重载 != 操作符
        public static bool operator !=(DiffFile p1, DiffFile p2)
        {
            // 如果 p1 或 p2 为 null，使用 Object.Equals 方法来比较
            if (p1 is null || p2 is null)
            {
                return !Equals(p1, p2);
            }

            // 否则，使用自定义的比较逻辑
            return !(p1.FileName == p2.FileName && p1.Path == p2.Path && p1.Hash == p2.Hash);
        }

        // 重载 == 操作符
        public static bool operator ==(DiffFile p1, DiffFile p2)
        {
            // 如果 p1 或 p2 为 null，使用 Object.Equals 方法来比较
            if (p1 is null || p2 is null)
            {
                return Equals(p1, p2);
            }

            // 否则，使用自定义的比较逻辑
            return p1.FileName == p2.FileName && p1.Path == p2.Path && p1.Hash == p2.Hash;
        }
        //重写Equals()方法
        public override bool Equals(object obj)
        {
            //如果obj为空或者类型不匹配，返回false
            if (obj == null || obj.GetType() != this.GetType())
                return false;
            //如果obj和this是同一个引用，返回true
            if (object.ReferenceEquals(obj, this))
                return true;
            //将obj转换为Student类型，并比较各个属性是否相等
            var other = obj as DiffFile;
            return this.FileName == other.FileName &&
                   this.Path == other.Path &&
                   this.Hash == other.Hash;
        }

        //重写GetHashCode()方法
        public override int GetHashCode()
        {
            //使用各个属性的哈希值进行异或运算，得到一个组合的哈希值
            return this.FileName.GetHashCode() ^
                   this.Path.GetHashCode() ^
                   this.Hash.GetHashCode();
        }
    }
}
