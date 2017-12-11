using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using NCStudio.WeChatService.Core;
using System.Text;

namespace NCStudio.WeChatService.App.Commands
{
    public class GenerateJSApiSignatureCommandHandler : IRequestHandler<GenerateJSApiSignatureCommand, string>
    {
        private IWeChatSecurity security;

        public GenerateJSApiSignatureCommandHandler(IWeChatSecurity security)
        {
            this.security = security ?? throw new ArgumentNullException(nameof(IWeChatSecurity));
        }

        public Task<string> Handle(GenerateJSApiSignatureCommand command, 
            CancellationToken cancellationToken=default(CancellationToken))
        {
            StringBuilder combineStringBuilder = new StringBuilder();
            combineStringBuilder.AppendFormat("{0}={1}", "jsapi_ticket", command.JsApiTicket);
            combineStringBuilder.AppendFormat("&");
            combineStringBuilder.AppendFormat("{0}={1}", "noncestr", security.GenerateNonceStr(16));
            combineStringBuilder.AppendFormat("&");
            combineStringBuilder.AppendFormat("{0}={1}", "timestamp", security.GetTimeStamp());
            combineStringBuilder.AppendFormat("&");
            combineStringBuilder.AppendFormat("{0}={1}", "url", command.Url);

            var combineString = combineStringBuilder.ToString();

            var signature = security.SHA1Hash(combineString);

            return Task.FromResult(signature);
        }
    }
}
