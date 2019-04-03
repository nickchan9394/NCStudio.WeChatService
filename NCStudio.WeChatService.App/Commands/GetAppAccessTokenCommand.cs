using MediatR;
using NCStudio.WeChatService.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace NCStudio.WeChatService.App.Commands
{
    public class GetAppAccessTokenCommand : IRequest<AppAccessToken>
    {
    }
}
