using Microsoft.Extensions.Configuration;
using NCStudio.WeChatService.Core;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace NCStudio.WeChatService.App
{
    public class WeChatCommunicator : IWeChatCommunicator
    {
        private IHttp http;
        private IConfiguration config;
        private IDateTimeProvider provider;

        public WeChatCommunicator(IHttp http, IConfiguration config,IDateTimeProvider provider)
        {
            this.http = http ?? throw new ArgumentNullException(nameof(IHttp));
            this.config = config ?? throw new ArgumentNullException(nameof(IConfiguration));
            this.provider = provider ?? throw new ArgumentNullException(nameof(IDateTimeProvider));
        }

        public async Task<AppAccessToken> GetAppAccessTokenAsync()
        {
            var result = JObject.Parse(await http.GetAsync($"https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={config["AppId"]}&secret={config["Secret"]}"));
            return new AppAccessToken
            {
                access_token = result["access_token"].Value<string>(),
                expires_in = result["expires_in"].Value<int>(),
                time_stamp=provider.GetNow()
            };
        }

        public async Task<UserAccessToken> GetUserAccessTokenAsync(string code) {
            return await http.GetAsync<UserAccessToken>($"https://api.weixin.qq.com/sns/oauth2/access_token?appid={config["AppId"]}&secret={config["Secret"]}&code={code}&grant_type=authorization_code");
        }

        public async Task<UserInfo> GetUserInfoAsync(string accessToken, string openId)
        {
            return await http.GetAsync<UserInfo>($"https://api.weixin.qq.com/sns/userinfo?access_token={accessToken}&openid={openId}&lang=zh_CN");
        }
    }
}
