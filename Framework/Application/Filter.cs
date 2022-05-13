namespace Framework.Application
{
    public class Filter
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string OrderBy { get; set; } //acs/dcs
        public string SortBy { get; set; }
    }
}