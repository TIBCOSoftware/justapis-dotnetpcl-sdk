using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APGW
{
    public interface IResponse
    {
        string RequestUri();

        Task<string> ReadResponseBodyAsString();

        Dictionary<string, List<string>> Headers();

        CacheControlOptions CacheControl();
              
    }

}

