using System.Threading.Tasks;


namespace APGW
{
    public interface IAPRestClient
    {
        Task<IResponse> ExecuteRequest<T>(RequestContext<T> request);
    }
}
