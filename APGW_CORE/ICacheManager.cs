
namespace APGW
{
    public interface ICacheManager
    {
        void PutIntoCache(string requestMethod, string url, string result);

        string GetFromCache(string requestMethod, string url);

    }
}
