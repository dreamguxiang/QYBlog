using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

//title: "{{ replace .Name "-" " " | title }}"
//slug:
//description:
//date: { { .Date } }
//lastmod: { { .Date } }
//image:
//math:
//license:
//hidden: false
//tags: []
//categories: []
//series: []
//comments: true
//draft: true

namespace QYBlog.Shared.Class
{
    public class Post
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }
        public string Lastmod { get; set; }
        public string Image { get; set; }
        public bool Math { get; set; }
        public string License { get; set; }
        public bool Hidden { get; set; }
        public List<string> Tags { get; set; }
        public List<string> Categories { get; set; }
        public List<string> Series { get; set; }
        public bool Comments { get; set; }
        public bool Draft { get; set; }
        public bool Password { get; set; }
        //路径
        public string Path { get; set; }

        public Post() { 
        }

        static public Post CreatePost(string a1,string a2)
        {
           var fileData =  File.ReadAllText(a1);
           //获取---与---之间的内容
           var PostInfo = fileData.Substring(fileData.IndexOf("---") + 3, fileData.LastIndexOf("---") - 3);

           //读取ymal
           var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            var post = deserializer.Deserialize<Post>(PostInfo);
            post.Path = a2;
            return post;
        }
    }
}
