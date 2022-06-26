namespace Framework.Helpers
{
    public abstract class PagingDataRequest
    {
        public int PageIndex { get; set; }

        private int _pageSize;
        public int PageSize
        {
            get => _pageSize == 0 ? 10 : _pageSize;
            set => _pageSize = value is 0 ? 10 : value;
        }

        public int TotalCount { get; set; }

        private string[] _orderBy;
        public string[] OrderBy
        {
            set => _orderBy = value;
            get =>
                _orderBy == null || _orderBy.Length == 0 ? new string[] { "Id Desc" } : _orderBy;
            
        }
    }
}