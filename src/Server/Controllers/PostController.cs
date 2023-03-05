using Microsoft.AspNetCore.Mvc;
using QYBlog.Shared.Class;


[Route("api/[controller]")]
[ApiController]
public class PostListController : ControllerBase
{
    private readonly List<Post> posts = new PostList(true).GetPostList();

    [HttpGet]
    public IEnumerable<Post> Get()
    {
        return posts;
    }

}
