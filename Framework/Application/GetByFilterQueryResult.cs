using System.Collections.Generic;

namespace Framework.Application
{
    public class GetByFilterQueryResult<T>
    {
        public List<T> Items { get; set; }
        public int TotalCount { get; set; }
    }
}