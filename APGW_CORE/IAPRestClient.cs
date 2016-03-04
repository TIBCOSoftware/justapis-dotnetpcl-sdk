using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Runtime;

namespace APGW
{
    public interface IAPRestClient
    {
        TransformedResponse<HttpResponseMessage> ReadResponse();

        Task<HttpResponseMessage> ExecuteRequest<T>(RequestContext<T> request);
    }
}
