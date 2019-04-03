using System;
using System.Collections.Generic;
using System.Text;

namespace NCStudio.WeChatService.Core
{
    public class AppAccessToken
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public DateTime time_stamp { get; set; }
    }
}
