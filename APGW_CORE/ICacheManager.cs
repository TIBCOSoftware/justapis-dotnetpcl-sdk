using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APGW
{
    public interface ICacheManager
    {
        void PutIntoCache(string requestMethod, string url, string result);

        string GetFromCache(string requestMethod, string url);

    }
}
