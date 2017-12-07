using System.Threading.Tasks;

namespace NCStudio.WeChatService.Core
{
    public interface IWeChatCommunicator
    {
        Task<UserAccessToken> GetUserAccessTokenAsync(string code);
    }
}