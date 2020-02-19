using System;
namespace PiBlog.Dto.Blog
{
    public class PostDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Body { get; set; }
        public DateTime CreationTime { get; set; }
    }
}