using AppCore.Contracts.Mediators;
using AppCore.Contracts.RepositoryBase;
using AppCore.Contracts.SpecificationBase;
using AppCore.DTOs;
using AppCore.Specifications;
using Mapster;
using Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.UseCases.DT.Queries
{
    public abstract class DTCostingSearch
    {
        public class Query : IListQuery<DTCostingSearchMainDto>
        {
            //public string Keyword { get; set; }

            public string? Keyword { get; set; }
            public List<string>? Includes { get; init; }
            public List<FilterModel>? Filters { get; init; }
            public List<string>? Sorts { get; init; }
            public int? Page { get; init; }
            public int? PageSize { get; init; }
        }
        public class Handler : IQueryHandler<Query, ListResultModel<DTCostingSearchMainDto>>
        {
            private readonly IDTCostingMainRepository _repository;

            public Handler(IDTCostingMainRepository repository)
            {
                _repository = repository;
            }

            /// <summary>
            ///     TODO: temp use this handler to loading temp tariff too, split it to another handler later
            /// </summary>
            /// <param name="query"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async ValueTask<ListResultModel<DTCostingSearchMainDto>> Handle(Query query,
                CancellationToken cancellationToken)
            {
                List<Entities.DTCostingMain> entity =
                    await _repository.FindAsync(new DTCosingSearchMainSpec(query.Keyword));
                long costingCount = await _repository.CountAsync(new DTCosingSearchMainSpec(query.Keyword));
                //return ResultModel<DTCostingSearchMainDto>.Create(entity?.Adapt<Entities.DTCostingMain>()!);
                return ListResultModel<DTCostingSearchMainDto>.Create(entity?.Adapt<List<DTCostingSearchMainDto>>()!, costingCount);
            }
        }

    }
}
