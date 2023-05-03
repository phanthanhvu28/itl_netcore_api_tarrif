using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Contracts.Common
{
    public abstract class AppDbContextBase : DbContext, IDbFacade
    {
        protected AppDbContextBase(DbContextOptions options) : base(options)
        {
        }
    }
}
