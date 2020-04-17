using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
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
        [Route("getpost")]
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

        [HttpPost]
        [Authorize]
        [Route("createpost")]
        public async Task<IActionResult> CreatePost([FromBody] PostDto postDto)
        {
            var resp = new Response<BlogDetailDto>();
            var retCode = await _postService.AddPost(postDto);
            if(retCode!=ErrorCode.OK)
            {
                resp.Msg = $"{retCode}";
                return Ok(resp);
            }

            var detailBlog = await _postService.GetBlogDetail(postDto.Id);
            if(detailBlog == null)
            {
                resp.Msg = $"Can not find blog by id:{postDto.Id}";
                return NotFound(resp);
            }

            resp.Content = detailBlog;
            return Ok(resp);
            
        }

        [HttpPut]
        [Authorize]
        [Route("updatepost")]
        public async Task<IActionResult> UpdatePost([FromBody] PostDto postDto)
        {
            var resp = new Response<BlogDetailDto>();
            var retCode = await _postService.UpdatePost(postDto);
            if(retCode!=ErrorCode.OK)
            {
                resp.Msg = $"{retCode}";
                return Ok(resp);
            }

            var detailBlog = await _postService.GetBlogDetail(postDto.Id);
            if(detailBlog == null)
            {
                resp.Msg = $"Can not find blog by id:{postDto.Id}";
                return NotFound(resp);
            }

            resp.Content = detailBlog;
            return Ok(resp);
            
        }

        [HttpDelete]
        [Authorize]
        [Route("deletepost")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var resp = new Response<string>();
            var retCode = await _postService.DeletePost(id);
            if(retCode!=ErrorCode.OK)
            {
                resp.Msg = $"id:{id} {retCode}";
                return Ok(resp);
            }

            return Ok(resp);
        }

    }
}