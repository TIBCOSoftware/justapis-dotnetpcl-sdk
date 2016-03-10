using System;

namespace APGW
{
    public class APGatewayBuilder<T> where T : APGateway, new()
    {
        public APGatewayBuilder ()
        {
        }
            
        private string _uri;
        public APGatewayBuilder<T> Uri(string uri)
        {
            _uri = uri;
            return this;
        }

        private string _method;
        public APGatewayBuilder<T> Method(string method)
        {
            _method = method;
            return this;
        }

        public T Build()
        {
            T gw = new T ();          

            gw.Uri = _uri;
            gw.Method = _method;

            return gw;
        }
    }
}

