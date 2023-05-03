using AppCore.Contracts.Mediators;
using AppCore.Contracts.RepositoryBase;
using AppCore.Specifications;
using AppCore.UseCases.DT.Models;
using Mapster;
using Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.UseCases.DT.Commands
{
    public abstract class DTCostingMain
    {
        public class Command : DTCosing, Contracts.Mediators.ICommand<bool>
        {
        }

        public class Handler : ICommandHandler<Command, ResultModel<bool>>
        {
            private readonly IDTCostingCapacityRepository _idtCostingCapacityRepository;
            private readonly IDTCostingMainRepository _idtCostingMainRepository;

            public Handler(IDTCostingMainRepository idtCostingMainRepository, IDTCostingCapacityRepository idtCostingCapacityRepository
                )
            {
                _idtCostingMainRepository = idtCostingMainRepository;
                _idtCostingCapacityRepository = idtCostingCapacityRepository;
            }

            public async ValueTask<ResultModel<bool>> Handle(Command command, CancellationToken cancellationToken)
            {
                Entities.DTCostingMain @new = command.Adapt<Entities.DTCostingMain>();
               _ = await MapCapacity(@new);
                _ = await _idtCostingMainRepository.AddAsync(@new);
                return ResultModel<bool>.Create(true);
            }

            private async Task<bool> MapCapacity(Entities.DTCostingMain item)
            {
                Entities.DTCostingCapacity? capacity =
                    await _idtCostingCapacityRepository.FindOneAsync(
                        new DTCostingCapacityGetByCapacityKeySpec(item.CapacityKey!));
                if (capacity is null)
                {
                    return false;
                }

                item.Capacity = capacity.CapacityValue;
                return true;
            }
        }
    }
}
