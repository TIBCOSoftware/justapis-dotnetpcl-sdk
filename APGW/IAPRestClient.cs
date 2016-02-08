using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APGW
{
    interface IAPRestClient
    {
        public void executeRequest<T>(RequestContext<T> request);
    }
}
