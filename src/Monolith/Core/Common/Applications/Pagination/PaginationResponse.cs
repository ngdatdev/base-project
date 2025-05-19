using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Applications.Pagination;

/// <summary>
/// This class is used to return a paged response
/// </summary>
/// <typeparam name="T"></typeparam>
public class PaginationResponse<T>
{
    public IEnumerable<T> Contents { get; init; }

    public int PageIndex { get; init; } = 1;

    public int PageSize { get; init; } = 20;

    public int TotalPages { get; init; }

    public bool HasPreviousPage => PageIndex > 1;

    public bool HasNextPage => PageIndex < TotalPages;
}
