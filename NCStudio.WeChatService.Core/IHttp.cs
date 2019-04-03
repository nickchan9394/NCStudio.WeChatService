using System.Threading.Tasks;

namespace NCStudio.WeChatService.Core
{
    public interface IHttp
    {
        Task<T> GetAsync<T>(string uri);
        /// <summary>
        /// Return json as Response body
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        Task<string> GetAsync(string uri);
    }
}
