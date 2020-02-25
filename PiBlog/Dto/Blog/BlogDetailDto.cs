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

        public string prevTitle { get; set; }
        public int prevId { get; set; }
        public string nextTitle { get; set; }
        public int nextId { get; set; }

    }
}