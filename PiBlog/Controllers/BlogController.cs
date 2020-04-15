using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using PiBlog.Dto;
using PiBlog.Dto.Blog;
using PiBlog.Interface;

namespace PiBlog.Controllers
{
    [Route("[controller]")]
    [Produces("application/json")]
    public class BlogController:ControllerBase
    {
        private readonly IPostService _postService;
        public BlogController(
            IPostService postService
        )
        {
            _postService= postService;
        }

        [HttpGet]
        [Route("query")]
        [ResponseCache(CacheProfileName="default",VaryByQueryKeys = new string[] { "page", "limit" })]
        public async Task<IActionResult> QueryPosts(QueryParameter input)
        {
            var posts =await _postService.GetPostList(input);
            var resp = new Response<PaginatedList<PostDto>>(posts);
            return Ok(resp);
        }

        [HttpGet("{id}")]
        [ResponseCache(CacheProfileName="default")]
        public async Task<IActionResult>Get(int id)
        {
            var resp = new Response<BlogDetailDto>();
            var detailBlog = await _postService.GetBlogDetail(id);
            if(detailBlog == null)
            {
                resp.Msg = $"Can not find blog by id:{id}";
                return NotFound(resp);
            }

            resp.Content = detailBlog;
            return Ok(resp);
        }

    }
}