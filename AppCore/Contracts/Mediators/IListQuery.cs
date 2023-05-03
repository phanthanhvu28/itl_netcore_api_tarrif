using AppCore.Contracts.SpecificationBase;
using Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Contracts.Mediators
{
    public interface IListQuery<TResponse> : IQuery<ListResultModel<TResponse>> where TResponse : notnull
    {
        public List<string> Includes { get; init; }

        public List<FilterModel>? Filters { get; init; }
        public List<string>? Sorts { get; init; }
        public int? Page { get; init; }
        public int? PageSize { get; init; }
    }
}
