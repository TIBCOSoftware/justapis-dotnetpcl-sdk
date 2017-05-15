using System.Collections.Generic;

namespace APGW
{
    public interface IRequestEncoding
    {
        string Encode(Dictionary<string,string> data);

        string Encoding();
    }
}

