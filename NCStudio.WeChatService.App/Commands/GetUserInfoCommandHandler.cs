using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NCStudio.WeChatService.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace NCStudio.WeChatService.App.Commands
{
    public class GetUserInfoCommandHandler:IRequestHandler<GetUserInfoCommand,UserInfo>
    {
        private IWeChatCommunicator communicator;

        public GetUserInfoCommandHandler(IWeChatCommunicator communicator)
        {
            this.communicator = communicator ?? throw new ArgumentNullException(nameof(IWeChatCommunicator));
        }

        public async Task<UserInfo> Handle(GetUserInfoCommand request, 
            CancellationToken cancellationToken=default(CancellationToken))
        {
            return await communicator.GetUserInfoAsync(request.AccessToken, request.OpenId);
        }
    }
}
