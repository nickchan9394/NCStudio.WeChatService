using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace NCStudio.WeChatService.Core
{
    public class WeChatCommunicator : IWeChatCommunicator
    {
        private IHttp http;
        private IConfiguration config;

        public WeChatCommunicator(IHttp http, IConfiguration config)
        {
            this.http = http ?? throw new ArgumentNullException(nameof(IHttp));
            this.config = config ?? throw new ArgumentNullException(nameof(IConfiguration));
        }

        public async Task<UserAccessToken> GetUserAccessTokenAsync(string code) {
            return await http.GetAsync<UserAccessToken>($"https://api.weixin.qq.com/sns/oauth2/access_token?appid={config["AppId"]}&secret={config["Secret"]}&code={code}&grant_type=authorization_code");
        }
    }
}
