using Microsoft.EntityFrameworkCore;
using NCStudio.Core;
using NCStudio.Utility.Mvc.Data;
using NCStudio.WeChatService.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace NCStudio.WeChatService.Data
{
    public interface IWeChatServiceContext:INCDbContext,IUnitOfWork
    {
        DbSet<UserAccessToken> UserAccessTokens { get; set; }
        DbSet<AppAccessToken> AppAccessTokens { get; set; }
    }
}
