using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NCStudio.WeChatService.App.Commands;

namespace NCStudio.WeChatService.App.Controllers
{
    [Route("api/[controller]")]
    public class UserInfoController:Controller
    {
        private IMediator mediator;

        public UserInfoController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("{code}")]
        public async Task<IActionResult> Get(string code)
        {
            var tokenResult = await mediator.Send(new GetUserAccessTokenCommand(code));
            return Ok(await mediator.Send(new GetUserInfoCommand(tokenResult.access_token, tokenResult.openid)));

        }
    }
}
