using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APGW
{
    public class TransformedResponse<T>
    {
        public T result;

        public TransformedResponse(T result) {
            this.result = result;
        }
    }
}
