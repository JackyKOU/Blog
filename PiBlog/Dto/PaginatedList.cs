using System.Collections.Generic;
namespace PiBlog.Dto
{
    public class PaginatedList<T>
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        private int _totalItemCount;
        public int TotalItemCount
        {
            get { return _totalItemCount; }
            set { _totalItemCount = value; }
        }

        public int PageCount
        {
            get { return TotalItemCount / PageSize + (TotalItemCount % PageSize == 0 ? 0 : 1); }
        }

        public bool HasPrevious
        {
            get { return PageIndex > 0; }
        }

        public bool HasNext
        {
            get { return PageIndex < PageCount - 1; }
        }

        public List<T> DataList
        {
            get;
            set;
        } = new List<T>();

        public PaginatedList(int pageIndex, int pageSize, int totalItemCount, List<T> dataList)
        {
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
            this.TotalItemCount = totalItemCount;
            DataList.AddRange(dataList);
        }
    }
}