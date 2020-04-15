using System;
namespace PiBlog.Dto
{
    public class Response
    {
        public string Msg
        {
            get;
            set;
        } = string.Empty;

        public bool Success
        {
            get
            {
                return string.IsNullOrEmpty(Msg);
            }
        }

        public DateTime Timestamp
        {
            get;
            set;
        } = DateTime.Now;
    }

    public class Response<T> : Response where T : class
    {
        public T Content { get; set; }

        public Response(T data)
        {
            this.Content = data;
        }

        public Response()
        {
            
        }
    }
}