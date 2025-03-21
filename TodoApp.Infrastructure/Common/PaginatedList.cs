using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

public class PaginatedList<T>
{
    public List<T> Data { get; }
    public int TotalCount { get; private set; }
    public int PageSize { get; private set; }
    public int CurrentPage { get; private set; }
    public int TotalPages { get; private set; }

    public PaginatedList(List<T> data, int count, int pageIndex, int pageSize)
    {
        TotalCount = count;
        PageSize = pageSize;
        CurrentPage = pageIndex;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        Data = data;
    }

    public bool HasPreviousPage => CurrentPage > 1;
    public bool HasNextPage => CurrentPage < TotalPages;

    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
    {
        var count = await source.CountAsync();
        var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
        return new PaginatedList<T>(items, count, pageIndex, pageSize);
    }
}
