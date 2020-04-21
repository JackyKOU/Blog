using System;
namespace PiBlog.Dto.Blog
{
    public class BlogDetailDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Body { get; set; }
        public DateTime CreationTime { get; set; }

        public string PrevTitle { get; set; }
        public int PrevId { get; set; }
        public string NextTitle { get; set; }
        public int NextId { get; set; }

    }
}