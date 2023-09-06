using TMC.Domain.Entities;

namespace TMC.Infrastructure.Pagination;

public class Pagination<T> where T : EntityBase<T>
{
    public Pagination()
    {
    }

    public Pagination(IEnumerable<T> items, int totalCount, int skip, int limit)
    {
        Items = items;
        NumberOfPages = totalCount / limit + 1;
        CurrentPage = skip / limit + 1;
        PageSize = limit;
    }

    public IEnumerable<T>? Items { get; set; }
    public int NumberOfPages { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }

    public void SetPage(int skip, int limit)
    {
        CurrentPage = skip / limit + 1;
        PageSize = limit;
    }
}