using Moq;
using NCStudio.Utility.Testing;
using NCStudio.WeChatService.App.Commands;
using NCStudio.WeChatService.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest.WeChatService.App
{
    public class GetUserInfoCommandHandler_UnitTest
    {
        Mock<IWeChatCommunicator> communicator = new Mock<IWeChatCommunicator>();

        [Fact]
        public void GetUserInfoCommandHandler_UnitTest_NullCheck()
        {
            //Assert
            Assert.Throws<ArgumentNullException>(nameof(IWeChatCommunicator),
                () => new GetUserInfoCommandHandler(null));
        }

        [Fact]
        public async Task Handle_GetUserInfoCommand()
        {
            //Arrange
            var expected = new UserInfo();
            var db = new List<UserInfo>();
            communicator.Setup(c => c.GetUserInfoAsync("token","openid"))
                .Returns(Task.FromResult(expected));

            var command = new GetUserInfoCommand(accessToken: "token",openId:"openid");
            var handler = createHandler();

            //Act
            var result = await handler.Handle(command);

            //Assert
            communicator.VerifyAll();
            Assert.True(Jsonning.EqualsOrThrows(expected, result));
        }

        private GetUserInfoCommandHandler createHandler()
        {
            return new GetUserInfoCommandHandler(communicator.Object);
        }
    }
}
