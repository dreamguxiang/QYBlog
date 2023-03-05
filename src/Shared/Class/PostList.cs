using System.Collections;

namespace QYBlog.Shared.Class
{
    public class PostList
    {
        private List<Post>? Posts;

        public PostList(bool isServer) {
            Posts = new List<Post>();
            if (isServer)
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

                //读取全部文章
                List<string> postDirs = Utils.Utils.GetDirectories("content/post");

                 foreach (var postDir in postDirs)
                {
                    //过滤非md
                    List<string> posts = Utils.Utils.GetFiles(postDir);
                    foreach (var post in posts)
                    {
                        if (post.EndsWith(".md"))
                        {
                            Posts.Add(Post.CreatePost(post));
                        }
                    }
                 }
            }
        }

        public PostList(List<Post> posts)
        {
            Posts = posts;
        }

        public List<Post> GetPostList()
        {
            if (Posts == null)
            {
                Posts = new List<Post>();
            }
            return Posts;
        }

        public bool Add(Post post)
        {
            if (Posts == null)
            {
                return false;
            }
            else
            {
                Posts.Add(post);
                return true;
            }
        }

        public bool Remove(Post post)
        {
            if (Posts == null)
            {
                return false;
            }
            else
            {
                Posts.Remove(post);
                return true;
            }
        }
    }
}
