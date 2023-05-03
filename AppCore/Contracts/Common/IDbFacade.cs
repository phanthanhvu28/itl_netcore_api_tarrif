using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Contracts.Common
{
   public interface IDbFacade
    {
        public DatabaseFacade Database { get; }
    }
}
