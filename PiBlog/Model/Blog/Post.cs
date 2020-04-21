using System;
namespace PiBlog.Model.Blog {
    public class Post : Entity {
        public string Title { get; set; }
        public string Author { get; set; }

        public string Body { get; set; }
        public string Summary { get; set; }
        public DateTime CreationTime { get; set; }
    }
}