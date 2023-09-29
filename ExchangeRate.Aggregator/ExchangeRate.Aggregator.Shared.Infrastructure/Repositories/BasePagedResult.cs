namespace ExchangeRate.Aggregator.Shared.Infrastructure.Repositories
{
    public abstract class BasePagedResult<T>
        where T : class
    {
        protected BasePagedResult()
            => Results = new List<T>();

        public List<T> Results { get; set; }

        public int PageNumber { get; set; }

        public int TotalPages { get; set; }

        public int PageSize { get; set; }

        public int TotalRecords { get; set; }

        public int FirstRowOnPage => TotalRecords != default ? ((PageNumber - 1) * PageSize) + 1 : default;

        public int LastRowOnPage => Math.Min(PageNumber * PageSize, TotalRecords);
    }
}
