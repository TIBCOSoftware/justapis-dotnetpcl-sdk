using System;

namespace APGW
{
    public class StringRequestContext : RequestContext<string>
    {
        public StringRequestContext() {
        }

        public StringRequestContext(HTTPMethod method, string url) : base(method, url) {}

        public override  TransformedResponse<string> ParseResponse(ResponseFromRequest responseFromRequest, Exception e) {
            return null;
        }

        public override TransformedResponse<string> ParseResponse(String rawResponseBody) {
            return new TransformedResponseString(rawResponseBody);
        }
    }
}
