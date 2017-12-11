using Microsoft.Extensions.Configuration;
using Moq;
using NCStudio.Utility.Testing;
using NCStudio.WeChatService.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest.WeChatService.Core
{
    public class WeChatCommunicator_UnitTest
    {
        Mock<IHttp> http = new Mock<IHttp>();
        IConfiguration config = new MockConfigruation();
        Mock<IDateTimeProvider> provider = new Mock<IDateTimeProvider>();

        [Fact]
        public void WeChatCommunicator_NullCheck()
        {
            //Assert
            Assert.Throws<ArgumentNullException>(nameof(IHttp),
                () => new WeChatCommunicator(null, config,provider.Object));
            Assert.Throws<ArgumentNullException>(nameof(IConfiguration),
                () => new WeChatCommunicator(http.Object, null,provider.Object));
            Assert.Throws<ArgumentNullException>(nameof(IDateTimeProvider),
                () => new WeChatCommunicator(http.Object, config, null));
        }

        [Fact]
        public async Task GetUserAccessTokenAsync_WeChatCommunicator()
        {
            //Arrange
            var expected = new UserAccessToken();
            var appid = "wxf1759596e3365d3e";
            var code = "123456";
            var secret = "50d58aee48c9e5cb8a593b93cc182547";

            var service = createService();
            http.Setup(h => h.GetAsync<UserAccessToken>($"https://api.weixin.qq.com/sns/oauth2/access_token?appid={appid}&secret={secret}&code={code}&grant_type=authorization_code"))
                .Returns(Task.FromResult(expected));
            config["AppId"] = appid;
            config["Secret"] = secret;
            //Act
            var actual = await service.GetUserAccessTokenAsync(code);

            //Assert
            http.VerifyAll();
            Assert.True(Jsonning.EqualsOrThrows(expected, actual));
        }

        [Fact]
        public async Task GetUserInfoAsync_WeChatCommunicator()
        {
            //Arrange
            var accessToken = "access token";
            var openId = "open id";
            var expected = new UserInfo();
            var service = createService();
            http.Setup(h => h.GetAsync<UserInfo>($"https://api.weixin.qq.com/sns/userinfo?access_token={accessToken}&openid={openId}&lang=zh_CN"))
                .Returns(Task.FromResult(expected));

            //Act
            var actual = await service.GetUserInfoAsync(accessToken,openId);

            //Assert
            http.VerifyAll();
            Assert.True(Jsonning.EqualsOrThrows(expected, actual));
        }

        [Fact]
        public async Task GetAppAccessTokenAsync_WeChatCommunicator()
        {
            //Arrange
            var appid = "wxf1759596e3365d3e";
            var secret = "50d58aee48c9e5cb8a593b93cc182547";
            var access_token = "123456";
            var expires_in = 7200;
            var expected = new AppAccessToken() {
                access_token=access_token,
                expires_in= expires_in,
                time_stamp=new DateTime(2017, 1, 1)
            };

            var service = createService();
            http.Setup(h => h.GetAsync($"https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={appid}&secret={secret}"))
                .Returns(Task.FromResult(JsonConvert.SerializeObject(new {
                    access_token=access_token,
                    expires_in=expires_in
                })));
            provider.Setup(p => p.GetNow()).Returns(new DateTime(2017,1,1));
            config["AppId"] = appid;
            config["Secret"] = secret;

            //Act
            var actual = await service.GetAppAccessTokenAsync();

            //Assert
            http.VerifyAll();
            provider.VerifyAll();
            Assert.True(Jsonning.EqualsOrThrows(expected, actual));
        }

        private IWeChatCommunicator createService()
        {
            return new WeChatCommunicator(http.Object,config,provider.Object);
        }
    }
}
