using MediatR;
using NCStudio.WeChatService.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using NCStudio.WeChatService.Data;

namespace NCStudio.WeChatService.App.Commands
{
    public class GetAppAccessTokenCommandHandler : IRequestHandler<GetAppAccessTokenCommand, AppAccessToken>
    {
        private IWeChatCommunicator communicator;
        private IWeChatServiceContext context;

        public GetAppAccessTokenCommandHandler(IWeChatCommunicator communicator, 
            IWeChatServiceContext context)
        {
            this.communicator = communicator ?? throw new ArgumentNullException(nameof(IWeChatCommunicator));
            this.context = context ?? throw new ArgumentNullException(nameof(IWeChatServiceContext));
        }

        public async Task<AppAccessToken> Handle(GetAppAccessTokenCommand command, 
            CancellationToken cancellationToken=default(CancellationToken))
        {
            AppAccessToken accessToken = null;

            var accessTokenInDb = context.AppAccessTokens.FirstOrDefault();
            if (accessTokenInDb != null)
            {
                if (DateTime.Now.Subtract(accessTokenInDb.time_stamp).TotalSeconds > accessTokenInDb.expires_in)
                {
                    accessToken = await communicator.GetAppAccessTokenAsync();
                    accessTokenInDb.access_token = accessToken.access_token;
                    accessTokenInDb.expires_in = accessToken.expires_in;
                    accessTokenInDb.time_stamp = accessToken.time_stamp;
                }
                else
                {
                    return accessTokenInDb;
                }
            }
            else
            {
                accessToken = await communicator.GetAppAccessTokenAsync();
                context.AppAccessTokens.Add(accessToken);
            }
            await context.SaveAsync();
            return accessToken;
        }
    }
}
