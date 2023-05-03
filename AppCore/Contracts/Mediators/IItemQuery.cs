using Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Contracts.Mediators
{
    public interface IItemQuery<TResponse> : IQuery<ResultModel<TResponse>>
      where TResponse : notnull
    {
    }
}
