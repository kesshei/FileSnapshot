using DiffUpdate.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DiffUpdate
{
    public static class FileDiff
    {
        public static string JsonName = "zipConfig.json";
        public static List<DiffFile> GetDiffInfo(string sourcePath, string targetPath)
        {
            var source = new DirectoryInfo(sourcePath).GetFiles("*.*", SearchOption.AllDirectories);
            var target = new DirectoryInfo(targetPath).GetFiles("*.*", SearchOption.AllDirectories);

            List<DiffFile> sourcefile = new List<DiffFile>();
            foreach (var item in source)
            {
                sourcefile.Add(GetFile(sourcePath, item));
            }
            List<DiffFile> targetfile = new List<DiffFile>();
            foreach (var item in target)
            {
                targetfile.Add(GetFile(targetPath, item));
            }
            var needAdd = sourcefile.Except(targetfile);
            var needDelete = targetfile.Except(sourcefile).ToList();
            foreach (var item in needDelete)
            {
                item.NeedDelete = true;
            }
            needDelete.AddRange(needAdd);

            var sourceDir = Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories).Select(t => GetDirectory(sourcePath, t));
            var targetDir = Directory.GetDirectories(targetPath, "*", SearchOption.AllDirectories).Select(t => GetDirectory(targetPath, t));

            var needDirAdd = sourceDir.Except(targetDir);
            var needDirDelete = targetDir.Except(sourceDir).ToList();
            foreach (var item in needDirDelete)
            {
                item.NeedDelete = true;
            }
            needDirDelete.AddRange(needDirAdd);
            needDelete.AddRange(needDirDelete);

            return needDelete;
        }
        private static DiffFile GetFile(string dir, FileInfo fileInfo)
        {
            string Hash256()
            {
                using (SHA1 mySHA1 = SHA1.Create())
                {
                    using (FileStream stream = File.OpenRead(fileInfo.FullName))
                    {
                        byte[] hashValue = mySHA1.ComputeHash(stream);
                        return BitConverter.ToString(hashValue).Replace("-", "");
                    }
                }
            }
            var hash = Hash256();
            var path = GetRelativePath(dir, fileInfo.FullName);
            return new DiffFile() { Path = path, Hash = hash, FileName = fileInfo.Name };
        }
        private static DiffFile GetDirectory(string dir, string dirPath)
        {
            var hash = "";
            var path = Path.GetRelativePath(dir, dirPath);
            return new DiffFile() { Path = path, Hash = hash, IsDirectory = true, FileName = Path.GetFileName(dirPath) };
        }
        public static string GetRelativePath(string rootPath, string filePath)
        {
            var path = filePath.Substring(rootPath.Length);
            if (path.StartsWith("\\"))
            {
                path = path.Substring(1);
            }
            return path;
        }
        public static void CopyDiffFileTo(List<DiffFile> fileDiffs, string source, string target)
        {
            Update(fileDiffs, source, target, false);
        }
        public static void CopyDiffFileToZip(List<DiffFile> fileDiffs, string source, string zipFile)
        {
            var json = fileDiffs.ToJson();
            if (File.Exists(zipFile))
            {
                File.Delete(zipFile);
            }
            using (ZipArchive archive = ZipFile.Open(zipFile, ZipArchiveMode.Create))
            {
                foreach (var item in fileDiffs.Where(t => !t.NeedDelete && !t.IsDirectory))
                {
                    var sourcePath = Path.Combine(source, item.Path).GetLongPath();
                    archive.CreateEntryFromFile(sourcePath, item.Hash);
                }
                ZipArchiveEntry entry = archive.CreateEntry(JsonName);
                using (StreamWriter writer = new StreamWriter(entry.Open()))
                {
                    writer.WriteLine(json);
                }
            }
        }

        public static List<DiffFile> ZipFileUpdateTo(string zipFile, string target)
        {
            List<DiffFile> fileDiffs = null;
            using (var zip = new ZipArchive(File.OpenRead(zipFile), ZipArchiveMode.Read, false))
            {
                if (zip != null && zip.Entries.Count > 0)
                {
                    var json = zip.GetEntry(JsonName);
                    if (json != null)
                    {
                        using (StreamReader reader = new StreamReader(json.Open()))
                        {
                            var data = reader.ReadToEnd();
                            fileDiffs = data.ToObj<List<DiffFile>>();
                        }
                    }
                    foreach (var item in fileDiffs.Where(t => t.NeedDelete))
                    {
                        var targetPath = Path.Combine(target, item.Path);
                        if (item.IsDirectory)
                        {
                            if (Directory.Exists(targetPath))
                            {
                                Directory.Delete(targetPath, true);
                            }
                        }
                        else
                        {
                            File.Delete(targetPath);
                        }
                    }
                    foreach (var item in fileDiffs.Where(t => !t.NeedDelete))
                    {
                        var path = Path.Combine(target, item.Path);
                        if (item.IsDirectory)
                        {
                            Directory.CreateDirectory(path);
                        }
                        else
                        {
                            var targetPath = path.GetLongPath();
                            FileInfo fileInfo = new FileInfo(targetPath);
                            if (!Directory.Exists(fileInfo.DirectoryName))
                            {
                                Directory.CreateDirectory(fileInfo.DirectoryName);
                            }
                            using (FileStream file = new FileStream(targetPath, FileMode.OpenOrCreate))
                            {
                                using (Stream stream = zip.GetEntry(item.Hash).Open())
                                {
                                    stream.CopyTo(file);
                                }
                            }
                        }
                    }
                }
            }
            return fileDiffs;
        }

        public static bool CheckUpdate(List<DiffFile> fileDiffs, string targetPath)
        {
            if (fileDiffs == null)
            {
                return false;
            }
            foreach (var item in fileDiffs)
            {
                var filePath = Path.Combine(targetPath, item.Path);
                if (item.NeedDelete)
                {
                    if (item.IsDirectory)
                    {
                        if (Directory.Exists(filePath))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (File.Exists(filePath))
                        {
                            var file = GetFile(targetPath, new FileInfo(filePath));
                            if (file == item)
                            {
                                return false;
                            }
                        }
                    }
                }
                else
                {
                    if (item.IsDirectory)
                    {
                        if (!Directory.Exists(filePath))
                        {
                            return false;
                        }
                    }
                    else
                    {

                        if (!File.Exists(filePath))
                        {
                            return false;
                        }
                        var file = GetFile(targetPath, new FileInfo(filePath));
                        if (file != item)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        public static void Update(List<DiffFile> fileDiffs, string source, string target, bool isUpdate = true)
        {
            if (isUpdate)
            {
                foreach (var item in fileDiffs.Where(t => t.NeedDelete))
                {
                    var targetPath = Path.Combine(target, item.Path);
                    File.Delete(targetPath);
                }
            }
            foreach (var item in fileDiffs.Where(t => !t.NeedDelete))
            {
                var sourcePath = Path.Combine(source, item.Path).GetLongPath();
                var targetPath = Path.Combine(target, item.Path).GetLongPath();
                FileInfo fileInfo = new FileInfo(targetPath);
                if (!Directory.Exists(fileInfo.DirectoryName))
                {
                    Directory.CreateDirectory(fileInfo.DirectoryName);
                }
                File.Copy(sourcePath, targetPath, true);
            }
        }
        //private static string GetTempPath()
        //{
        //    var tempDirName = Guid.NewGuid().ToString("N");
        //    var temp = Temp();
        //    var tempDir = Path.Combine(temp, tempDirName);
        //    if (!Directory.Exists(tempDir))
        //    {
        //        Directory.CreateDirectory(tempDir);
        //    }
        //    return tempDir;
        //}
        //private static string Temp()
        //{
        //    var temp = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp");
        //    if (!Directory.Exists(temp))
        //    {
        //        Directory.CreateDirectory(temp);
        //    }
        //    return temp;
        //}
        //public static List<DiffFile> ZipFileUpdateTo1(string zipFile, string targetPath)
        //{
        //    var temp = GetTempPath();
        //    ZipFileHelper.Decompression(zipFile, temp);
        //    var dirPath = Directory.GetDirectories(temp).First();
        //    var jsonPath = Path.Combine(dirPath, JsonName);
        //    List<DiffFile> fileDiffs = File.ReadAllText(jsonPath).ToObj<List<DiffFile>>();
        //    Update(fileDiffs, dirPath, targetPath);
        //    Directory.Delete(temp, true);
        //    return fileDiffs;
        //}
        //private static void DeleteDirectory(string dir)
        //{
        //    var source = new DirectoryInfo(dir).GetFiles("*.*", SearchOption.AllDirectories);
        //    foreach (var item in source)
        //    {
        //        if (item.IsReadOnly)
        //        {
        //            item.IsReadOnly = false;
        //        }
        //        File.Delete(item.FullName.GetLongPath());
        //    }
        //    Directory.Delete(dir, true);
        //}
    }
}
