using NCStudio.Utility.Mvc.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using NCStudio.WeChatService.Core;

namespace NCStudio.WeChatService.Data
{
    public class WeChatServiceContext : NCDbContext, IWeChatServiceContext
    {
        public DbSet<UserAccessToken> UserAccessTokens { get; set; }
        public DbSet<AppAccessToken> AppAccessTokens { get; set; }
    }
}
