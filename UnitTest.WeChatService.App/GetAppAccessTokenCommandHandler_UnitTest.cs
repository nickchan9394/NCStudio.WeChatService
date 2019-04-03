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

namespace UnitTest.WeChatService.App
{
    public class GetAppAccessTokenCommandHandler_UnitTest
    {
        Mock<IWeChatCommunicator> communicator = new Mock<IWeChatCommunicator>();
        Mock<IWeChatServiceContext> context = new Mock<IWeChatServiceContext>();

        [Fact]
        public void GetAppAccessTokenCommandHandler_NullCheck()
        {
            //Assert
            Assert.Throws<ArgumentNullException>(nameof(IWeChatCommunicator),
                () => new GetAppAccessTokenCommandHandler(null, context.Object));
            Assert.Throws<ArgumentNullException>(nameof(IWeChatServiceContext),
                () => new GetAppAccessTokenCommandHandler(communicator.Object, null));
        }

        [Fact]
        public async Task Handle_GetAppAccessTokenCommand_AccessTokenNotExisted()
        {
            //Arrange
            var expected = new AppAccessToken();
            var db = new List<AppAccessToken>();
            communicator.Setup(c => c.GetAppAccessTokenAsync())
                .Returns(Task.FromResult(expected));
            context.SetupGet(c => c.AppAccessTokens).Returns(Mocking.GetMockDbSet(db));
            context.Setup(c => c.SaveAsync()).Returns(Task.CompletedTask);

            var command = new GetAppAccessTokenCommand();
            var handler = createHandler();

            //Act
            var result = await handler.Handle(command);

            //Assert
            communicator.VerifyAll();
            context.VerifyAll();
            Assert.True(Jsonning.EqualsOrThrows(expected, result));
            Assert.Same(expected, db.FirstOrDefault());
        }

        [Fact]
        public async Task Handle_GetAppAccessTokenCommand_AccessTokenExisted()
        {
            //Arrange
            var expected = new AppAccessToken()
            {
                access_token = "789012",
                expires_in = 7200,
                time_stamp = DateTime.Now
            };
            var db = new List<AppAccessToken>() { expected };
            context.SetupGet(c => c.AppAccessTokens).Returns(Mocking.GetMockDbSet(db));

            var command = new GetAppAccessTokenCommand();
            var handler = createHandler();

            //Act
            var result = await handler.Handle(command);

            //Assert
            context.VerifyAll();
            Assert.True(Jsonning.EqualsOrThrows(expected, result));
            Assert.Same(expected, db.FirstOrDefault());
        }

        [Fact]
        public async Task Handle_GetAppAccessTokenCommand_AccessTokenOutDated()
        {
            //Arrange
            var expected = new AppAccessToken() {
                access_token = "789012",
                expires_in = 7200,
                time_stamp = DateTime.Now
            };
            var db = new List<AppAccessToken>() {
                new AppAccessToken{
                    access_token="123456",
                    expires_in=7200,
                    time_stamp=DateTime.Now.AddHours(-3)
                }
            };
            communicator.Setup(c => c.GetAppAccessTokenAsync())
                .Returns(Task.FromResult(expected));
            context.SetupGet(c => c.AppAccessTokens).Returns(Mocking.GetMockDbSet(db));
            context.Setup(c => c.SaveAsync()).Returns(Task.CompletedTask);

            var command = new GetAppAccessTokenCommand();
            var handler = createHandler();

            //Act
            var result = await handler.Handle(command);

            //Assert
            communicator.VerifyAll();
            context.VerifyAll();
            Assert.True(Jsonning.EqualsOrThrows(expected, result));
            Assert.Equal(expected.access_token, db.FirstOrDefault().access_token);
            Assert.Equal(expected.expires_in, db.FirstOrDefault().expires_in);
            Assert.Equal(expected.time_stamp, db.FirstOrDefault().time_stamp);
        }

        private GetAppAccessTokenCommandHandler createHandler()
        {
            return new GetAppAccessTokenCommandHandler(communicator.Object, context.Object);
        }
    }
}
