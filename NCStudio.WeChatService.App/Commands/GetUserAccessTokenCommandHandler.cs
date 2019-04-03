using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using NCStudio.WeChatService.Core;
using NCStudio.WeChatService.Data;
using Microsoft.EntityFrameworkCore;

namespace NCStudio.WeChatService.App.Commands
{
    public class GetUserAccessTokenCommandHandler : 
        IRequestHandler<GetUserAccessTokenCommand, UserAccessToken>
    {
        private IWeChatCommunicator communicator;
        private IWeChatServiceContext context;

        public GetUserAccessTokenCommandHandler(IWeChatCommunicator communicator, 
            IWeChatServiceContext context)
        {
            this.communicator = communicator ?? throw new ArgumentNullException(nameof(IWeChatCommunicator));
            this.context = context ?? throw new ArgumentNullException(nameof(IWeChatServiceContext));
        }

        public async Task<UserAccessToken> Handle(GetUserAccessTokenCommand command,
            CancellationToken cancellationToken=default(CancellationToken))
        {
            var userAccessToken= await communicator.GetUserAccessTokenAsync(command.Code);

            var tokenInDb = await context.UserAccessTokens.FirstOrDefaultAsync(t => t.openid == userAccessToken.openid);
            if (tokenInDb == null)
            {
                context.UserAccessTokens.Add(userAccessToken);
            }
            else
            {
                tokenInDb.access_token = userAccessToken.access_token;
                tokenInDb.expires_in = userAccessToken.expires_in;
                tokenInDb.refresh_token = userAccessToken.refresh_token;
                tokenInDb.scope = userAccessToken.scope;
            }
            await context.SaveAsync();

            return userAccessToken;
        }
    }
}
