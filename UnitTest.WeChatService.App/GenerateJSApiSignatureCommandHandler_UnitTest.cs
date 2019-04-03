using Moq;
using NCStudio.WeChatService.App.Commands;
using NCStudio.WeChatService.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest.WeChatService.App
{
    public class GenerateJSApiSignatureCommandHandler_UnitTest
    {
        Mock<IWeChatSecurity> security = new Mock<IWeChatSecurity>();

        [Fact]
        public void GenerateJSApiSignatureCommandHandler_NullCheck()
        {
            //Assert
            Assert.Throws<ArgumentNullException>(nameof(IWeChatSecurity),
                () => new GenerateJSApiSignatureCommandHandler(null));
        }

        [Fact]
        public async Task Handle_GenerateJSApiSignatureCommandHandler_UnitTest()
        {
            //Arrange
            var jsApiTicket = "ticket";
            var url = "url";
            security.Setup(s => s.GenerateNonceStr(16)).Returns("noncestr");
            security.Setup(s => s.GetTimeStamp()).Returns(12355468554856);
            security.Setup(s => s.SHA1Hash($"jsapi_ticket={jsApiTicket}&noncestr={"noncestr"}&timestamp={12355468554856}&url={url}"))
                .Returns("hashed");

            var command = new GenerateJSApiSignatureCommand(jsApiTicket, url);
            var handler = createHandler();

            //Act
            var result = await handler.Handle(command);

            //Assert
            security.VerifyAll();
            Assert.Equal("hashed", result);
        }

        private GenerateJSApiSignatureCommandHandler createHandler()
        {
            return new GenerateJSApiSignatureCommandHandler(security.Object);
        }
    }
}
