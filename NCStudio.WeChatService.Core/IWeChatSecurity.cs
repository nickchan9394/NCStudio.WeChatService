using System;
using System.Collections.Generic;
using System.Text;

namespace NCStudio.WeChatService.Core
{
    public interface IWeChatSecurity
    {
        string GenerateNonceStr(int length);
        long GetTimeStamp();
        string SHA1Hash(string plain);
    }
}
