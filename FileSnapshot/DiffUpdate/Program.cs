using System;
using System.IO;

namespace DiffUpdate
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Test1();


            Console.ReadLine();
        }
        static void Test1()
        {
            //根据新版本和老版本，找出差异，生成ZIP差异包，让服务器自己根据差异包自己更新
            var NewPath = @"E:\git\KuaPingTai\Xamarin.Android.SDK";
            var OldPath = @"E:\git\KuaPingTai\新建文件夹";
            var ZipPath = @"E:\git\KuaPingTai\123.zip";
            var ServerPath = @"E:\git\KuaPingTai\ServerPath";
            var diffs = FileDiff.GetDiffInfo(NewPath, OldPath);
            if (diffs?.Count == 0)
            {
                Console.WriteLine("双方内容一致，无法打包!");
            }
            else
            {
                FileDiff.CopyDiffFileToZip(diffs, NewPath, ZipPath);
                //服务端需要的
                var updateDiffs = FileDiff.ZipFileUpdateTo(ZipPath, ServerPath);
                var state = FileDiff.CheckUpdate(updateDiffs, ServerPath);
                Console.WriteLine($"文件更新状态:{state}");
            }
        }
        static void Test()
        {
            var path1 = @"C:\Users\Win\Desktop\source12";
            var path2 = @"C:\Users\Win\Desktop\source11";

            var source2 = @"C:\Users\Win\Desktop\新建文件夹";
            var zipFIle = @"C:\Users\Win\Desktop\123.zip";
            //var list = FileDiff.GetDiffFile(path1, path2);
            //FileDiff.CopyDiffFileToZip(list, path1, zipFIle);

            //FileDiff.ZipFileUpdateTo(zipFIle, path2);
            //FileDiff.ZipFileUpdateTo1(zipFIle, path2);
            //FileDiff.Update(list, path1, path2);

            //var bb = FileDiff.GetDiffInfo(path1, path2);


            var a1 = "\\\\?\\E:\\BaiduSyncdisk\\临时项目\\DiffUpdate\\DiffUpdate\\bin\\Debug\\temp\\7d6705f0e10944d18671a89990700a2b\\f3172ca584e648c4873395d5d3907f5c\\Xamarin.Android.BaseRecyclerViewAdapterHelper\\src\\Xamarin.Android.RecyclerView\\Xamarin.Android.RecyclerView\\Xamarin.Android.RecyclerView.csproj";
            var a2 = "E:\\git\\KuaPingTai\\ServerPath\\Xamarin.Android.BaseRecyclerViewAdapterHelper\\src\\Xamarin.Android.RecyclerView\\Xamarin.Android.RecyclerView\\Xamarin.Android.RecyclerView.csproj";
            //using (FileStream file = new FileStream(a2, FileMode.OpenOrCreate))
            //{
            //    using (FileStream stream = new FileStream(a1, FileMode.Open))
            //    {
            //        stream.CopyTo(file);
            //    }
            //}

            var source = "E:\\git\\KuaPingTai\\Xamarin.Android.SDK\\Xamarin.Android.ZXing.Android.Embedded\\src\\Xamarin.Android.ZXing.Android.Embedded\\obj\\Debug\\generated\\src\\Com.Google.Zxing.Client.Android.Camera.Open.OpenCameraInterface.cs";
            var path = "E:\\BaiduSyncdisk\\临时项目\\DiffUpdate\\DiffUpdate\\bin\\Debug\\temp\\c68d4c0bb2af449e82c4a6aba4358b20\\Xamarin.Android.ZXing.Android.Embedded\\src\\Xamarin.Android.ZXing.Android.Embedded\\obj\\Debug\\generated\\src\\Com.Google.Zxing.Client.Android.Camera.CameraConfigurationUtils.cs";
            var exsit = File.Exists(source);
            var exsit2 = Directory.Exists(path);
            var exsit3 = Directory.Exists(new FileInfo(path).DirectoryName);
            if (!exsit3)
            {
                Directory.CreateDirectory(new FileInfo(path).DirectoryName);
            }
            //var path4 = $"\\\\?\\{path}";
            //using (FileStream file = new FileStream(path, FileMode.OpenOrCreate))
            //{
            //    using (FileStream stream = new FileStream(source, FileMode.Open))
            //    {
            //        stream.CopyTo(file);
            //    }
            //}
            //RegeditHelper.SetLongPath();
            //File.Copy(source, path, true);

        }
    }
}
