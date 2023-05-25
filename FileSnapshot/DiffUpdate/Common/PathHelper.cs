using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffUpdate.Common
{
    public static class PathHelper
    {
        public static int MAX_PATH = 260;
        public static string GetLongPath(this string path)
        {
            if (path?.Length >= MAX_PATH)
            {
                return $"\\\\?\\{path}";
            }
            return path;
        }
    }
}
