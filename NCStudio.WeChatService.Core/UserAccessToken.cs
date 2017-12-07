using System;
using System.Collections.Generic;
using System.Text;

namespace NCStudio.WeChatService.Core
{
    public class UserAccessToken
    {
        public string access_token { get; set; }
        public string expires_in { get; set; }
        public string refresh_token { get; set; }
        public string scope { get; set; }

        public string openid { get; set; }
    }
}
