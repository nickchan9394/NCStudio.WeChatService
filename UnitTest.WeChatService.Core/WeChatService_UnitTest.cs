using Microsoft.Extensions.Configuration;
using Moq;
using NCStudio.Utility.Testing;
using NCStudio.WeChatService.Core;
using NCStudio.WeChatService.Data;
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

        [Fact]
        public void WeChatCommunicator_NullCheck()
        {
            //Assert
            Assert.Throws<ArgumentNullException>(nameof(IHttp),
                () => new WeChatCommunicator(null, config));
            Assert.Throws<ArgumentNullException>(nameof(IConfiguration),
                () => new WeChatCommunicator(http.Object, null));
        }

        [Fact]
        public async Task GetUserAccessTokenAsync_WeChatCommunicator_UnitTest()
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

        private IWeChatCommunicator createService()
        {
            return new WeChatCommunicator(http.Object,config);
        }
    }
}
