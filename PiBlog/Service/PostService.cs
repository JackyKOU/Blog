using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PiBlog.Infrastructure;
using PiBlog.Dto;
using PiBlog.Dto.Blog;
using PiBlog.Model.Blog;
using System.Linq;
using PiBlog.Interface;

namespace PiBlog.Service
{
    public class PostService:IPostService
    {
        private readonly PiDbContext _piDbContext;

        public PostService(PiDbContext piDbContext)
        {
            _piDbContext = piDbContext;
        }

        public async Task<PaginatedList<PostDto>> GetPostList(QueryParameter input)
        {
            var posts = await _piDbContext.Posts.ToListAsync();
            int count = posts.Count;

            var queryPosts = posts.OrderByDescending(x=>x.CreationTime)
                                  .Skip((input.PageIndex-1)* input.PageSize)
                                  .Take(input.PageSize)
                                  .Select(x=>new PostDto
                                  {
                                      Id = x.Id,
                                      Title = x.Title,
                                      Author = x.Author,
                                      Body = x.Body,
                                      CreationTime = x.CreationTime
                                  }).ToList();

            var pagePostList = new PaginatedList<PostDto>(input.PageSize,input.PageSize,count,queryPosts);
            return pagePostList;                  
        }
    }
}