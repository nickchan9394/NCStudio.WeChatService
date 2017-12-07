using Microsoft.AspNetCore.Mvc;
using Moq;
using NCStudio.Utility.Testing;
using NCStudio.WeChatService.App.Commands;
using NCStudio.WeChatService.Core;
using NCStudio.WeChatService.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest.WeChatService.Core
{
    public class GetUserAccessTokenCommandHandler_UnitTest
    {
        Mock<IWeChatCommunicator> communicator = new Mock<IWeChatCommunicator>();
        Mock<IWeChatServiceContext> context = new Mock<IWeChatServiceContext>();

        [Fact]
        public void GetUserAccessTokenCommandHandler_NullCheck()
        {
            //Assert
            Assert.Throws<ArgumentNullException>(nameof(IWeChatCommunicator),
                () => new GetUserAccessTokenCommandHandler(null,context.Object));
            Assert.Throws<ArgumentNullException>(nameof(IWeChatServiceContext),
                () => new GetUserAccessTokenCommandHandler(communicator.Object, null));
        }
        [Fact]
        public async Task Handle_GetUserAccessTokenCommand_OpenIdOfUserAccessTokenNotExisted()
        {
            //Arrange
            var expected = new UserAccessToken();
            var db = new List<UserAccessToken>();
            communicator.Setup(c => c.GetUserAccessTokenAsync("123"))
                .Returns(Task.FromResult(expected));
            context.SetupGet(c => c.UserAccessTokens).Returns(Mocking.GetMockDbSet(db));
            context.Setup(c => c.SaveAsync()).Returns(Task.CompletedTask);

            var command = new GetUserAccessTokenCommand(code:"123");
            var handler = createHandler();

            //Act
            var result = await handler.Handle(command);

            //Assert
            communicator.VerifyAll();
            context.VerifyAll();
            Assert.True(Jsonning.EqualsOrThrows(expected,result));
            Assert.Same(expected, db.FirstOrDefault());
        }

        [Fact]
        public async Task Handle_GetUserAccessTokenCommand_OpenIdOfUserAccessTokenExisted()
        {
            //Arrange
            var expected = new UserAccessToken()
            {
                openid = "321",
                access_token="1",
                expires_in="1",
                refresh_token="1",
                scope="1"
            };
            var db = new List<UserAccessToken>{new UserAccessToken
            {
                openid = "321",
                access_token="2",
                expires_in="2",
                refresh_token="2",
                scope="2"
            } };
            communicator.Setup(c => c.GetUserAccessTokenAsync("123"))
                .Returns(Task.FromResult(expected));
            context.SetupGet(c => c.UserAccessTokens).Returns(Mocking.GetMockDbSet(db));
            context.Setup(c => c.SaveAsync()).Returns(Task.CompletedTask);

            var command = new GetUserAccessTokenCommand(code: "123");
            var handler = createHandler();

            //Act
            var result = await handler.Handle(command);

            //Assert
            communicator.VerifyAll();
            context.VerifyAll();
            Assert.True(Jsonning.EqualsOrThrows(expected, result));
            Assert.NotSame(expected, db.FirstOrDefault());
            Assert.Equal(expected.access_token, db.FirstOrDefault().access_token);
            Assert.Equal(expected.expires_in, db.FirstOrDefault().expires_in);
            Assert.Equal(expected.refresh_token, db.FirstOrDefault().refresh_token);
            Assert.Equal(expected.scope, db.FirstOrDefault().scope);

        }

        private GetUserAccessTokenCommandHandler createHandler()
        {
            return new GetUserAccessTokenCommandHandler(communicator.Object,context.Object);
        }
    }
}
