using NCStudio.WeChatService.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace NCStudio.WeChatService.App.Infrastructures
{
    public class Http : IHttp
    {
        public async Task<T> GetAsync<T>(string uri)
        {
            using(var httpClient=new HttpClient())
            {
                var response=await httpClient.GetAsync(uri);
                var content = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return JsonConvert.DeserializeObject<T>(content);
                }
                else
                {
                    throw new Exception($"WeChatServer Error:{{code:{response.StatusCode},message:{content}}}");
                }
            }
        }

        public async Task<string> GetAsync(string uri)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(uri);
                var content = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return content;
                }
                else
                {
                    throw new Exception($"WeChatServer Error:{{code:{response.StatusCode},message:{content}}}");
                }
            }
        }
    }
}
