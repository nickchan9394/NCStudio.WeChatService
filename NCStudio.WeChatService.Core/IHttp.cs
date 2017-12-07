using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NCStudio.WeChatService.Core
{
    public interface IHttp
    {
        Task<T> GetAsync<T>(string uri);
    }
}
