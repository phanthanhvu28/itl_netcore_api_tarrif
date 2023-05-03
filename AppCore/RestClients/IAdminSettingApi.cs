using RestEase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.RestClients
{
    public interface IAdminSettingApi
    {
        [Get]
        public Task<HttpResponseMessage> GetProcessFlow();
    }
}
