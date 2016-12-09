using System.Threading.Tasks;
using System.Net.Http;

namespace APGW
{
    public abstract class TransformedResponse<T>
    {
        public T Result { get; set; }

        public TransformedResponse() {}


        public TransformedResponse(T result) {
            this.Result = result;
        }

        public abstract Task<string> ReadResponseBodyAsString ();
    }

    public class TransformedResponseHttpClient: TransformedResponse<HttpResponseMessage> {
        public TransformedResponseHttpClient() {
        }

        public TransformedResponseHttpClient(HttpResponseMessage result) {
            this.Result = result;
        }

        public override async Task<string> ReadResponseBodyAsString() {
            return await Result.Content.ReadAsStringAsync ();
        }
    }


    public class TransformedResponseString: TransformedResponse<string> {
        public TransformedResponseString() {}

        public TransformedResponseString(string result) {
            this.Result = result;
        }

        public override async Task<string> ReadResponseBodyAsString() {
            return null;
        }


    }
}
