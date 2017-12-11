using MediatR;
using NCStudio.WeChatService.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace NCStudio.WeChatService.App.Commands
{
    [DataContract]
    public class GetUserInfoCommand : IRequest<UserInfo>
    {
        [DataMember]
        public string AccessToken { get; private set; }
        [DataMember]
        public string OpenId { get; private set; }

        public GetUserInfoCommand(string accessToken, string openId)
        {
            this.AccessToken = accessToken;
            this.OpenId = openId;
        }
    }
}
