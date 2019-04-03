using MediatR;
using Microsoft.AspNetCore.Mvc;
using NCStudio.WeChatService.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace NCStudio.WeChatService.App.Commands
{
    [DataContract]
    public class GetUserAccessTokenCommand : IRequest<UserAccessToken>
    {
        [DataMember]
        public string Code { get; private set; }

        public GetUserAccessTokenCommand(string code)
        {
            this.Code = code;
        }
    }
}
