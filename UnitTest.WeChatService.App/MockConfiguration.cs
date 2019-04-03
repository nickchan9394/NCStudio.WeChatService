using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTest.WeChatService.App
{
    public class MockConfigruation : IConfiguration
    {
        private Dictionary<string, string> dictionary = new Dictionary<string, string>();
        public string this[string key]
        {
            get => dictionary.ContainsKey(key) ? dictionary[key] : null;
            set => dictionary.Add(key, value);
        }

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            throw new NotImplementedException();
        }

        public IChangeToken GetReloadToken()
        {
            throw new NotImplementedException();
        }

        public IConfigurationSection GetSection(string key)
        {
            throw new NotImplementedException();
        }
    }
}
