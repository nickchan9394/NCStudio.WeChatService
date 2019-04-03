using System;
using System.Collections.Generic;
using System.Text;

namespace NCStudio.WeChatService.Core
{
    public interface IDateTimeProvider
    {
        DateTime GetNow();
    }
}
