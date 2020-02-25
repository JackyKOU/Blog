using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PiBlog.Infrastructure;
using PiBlog.Dto;
using PiBlog.Dto.Blog;
using PiBlog.Model.Blog;
using System.Linq;
namespace PiBlog.Interface
{
    public interface IPostService
    {
        Task<PaginatedList<PostDto>> GetPostList(QueryParameter input);

        Task<BlogDetailDto> GetBlogDetail(int id);
    }
}