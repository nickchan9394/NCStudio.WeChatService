using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace NCStudio.WeChatService.App.Commands
{
    public class GenerateJSApiSignatureCommand : IRequest<string>
    {
        public string JsApiTicket { get; private set; }
        public string Url { get; private set; }
        public GenerateJSApiSignatureCommand(string jsApiTicket,string url)
        {
            JsApiTicket = jsApiTicket;
            Url = url;
        }
    }
}
