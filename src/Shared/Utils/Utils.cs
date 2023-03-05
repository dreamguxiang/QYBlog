namespace QYBlog.Shared.Utils
{
    public class Utils
    {
        //获取文件夹所有文件，不包括子文件夹
        public static List<string> GetFiles(string path)
        {
            List<string> files = new List<string>();
            if (Directory.Exists(path))
            {
                files.AddRange(Directory.GetFiles(path));
            }
            return files;
        }

        //获取文件夹内所有文件夹，不包括文件
        public static List<string> GetDirectories(string path)
        {
            List<string> directories = new List<string>();
            if (Directory.Exists(path))
            {
                directories.AddRange(Directory.GetDirectories(path));
            }
            return directories;
        }

        public static void CreateDirectory()
        {
            //检测content文件夹是否存在，否则创建
            if (!System.IO.Directory.Exists("content"))
            {
                System.IO.Directory.CreateDirectory("content");
            }
            //检测content/post是否存在
            if (!System.IO.Directory.Exists("content/post"))
            {
                System.IO.Directory.CreateDirectory("content/post");
            }
        }
    }
}
