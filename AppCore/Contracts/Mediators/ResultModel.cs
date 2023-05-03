using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Contracts.Mediators
{
    public record ResultModel<T>
    {
        public T Data { get; private init; }
        public bool IsError { get; private init; }
        public string ErrorMessage { get; private init; }

        public static ResultModel<T> Create(T data, bool isError = false, string errorMessage = default!)
        {
            return new()
            {
                Data = data,
                IsError = isError,
                ErrorMessage = errorMessage
            };
        }
    }
}
public record ListResultModel<T>
{
    public List<T> Items { get; private init; }
    public long TotalItems { get; private init; }
    public int Page { get; private init; }
    public int PageSize { get; private init; }

    public static ListResultModel<T> Create(List<T> items, long totalItems = 0, int page = 1, int pageSize = 20)
    {
        return new()
        {
            Items = items,
            TotalItems = totalItems,
            Page = page,
            PageSize = pageSize
        };
    }
}
