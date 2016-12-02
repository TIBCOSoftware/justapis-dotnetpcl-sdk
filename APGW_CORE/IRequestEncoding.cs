using System;
using System.Collections.Generic;

namespace APGW
{
    public interface IRequestEncoding
    {
        string Encode(Dictionary<string,object> data);

        string Encoding();
    }
}

