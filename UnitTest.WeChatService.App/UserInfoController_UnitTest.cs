using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NCStudio.Utility.Testing;
using NCStudio.WeChatService.App.Commands;
using NCStudio.WeChatService.App.Controllers;
using NCStudio.WeChatService.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest.WeChatService.App
{
    public class UserInfoController_UnitTest
    {
        Mock<IMediator> mediator = new Mock<IMediator>();

        [Fact]
        public async Task Get_UserInfoController()
        {
            //Arrange
            var code = "123";
            var expected = new UserInfo();
            var controller = createController();
            mediator.Setup(m => m.Send(It.IsAny<GetUserAccessTokenCommand>(),It.IsAny<CancellationToken>()))
                .Returns<GetUserAccessTokenCommand,CancellationToken>((command,token) => {
                Assert.Equal(code, command.Code);
                return Task.FromResult(new UserAccessToken()
                {
                    access_token = "access token",
                    openid = "open id"
                });
            });
            mediator.Setup(m => m.Send(It.IsAny<GetUserInfoCommand>(), It.IsAny<CancellationToken>()))
               .Returns<GetUserInfoCommand, CancellationToken>((command, token) => {
                   Assert.Equal("access token", command.AccessToken);
                   Assert.Equal("open id", command.OpenId);
                   return Task.FromResult(expected);
               });

            //Act
            var result = await controller.Get(code) as OkObjectResult;

            //Assert
            Assert.True(Jsonning.EqualsOrThrows(expected, result.Value));
        }

        private UserInfoController createController()
        {
            return new UserInfoController(mediator.Object);
        }
    }
}
