using NCStudio.WeChatService.Core;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace NCStudio.WeChatService.App
{
    public class WeChatSecurity : IWeChatSecurity
    {
        public string GenerateNonceStr(int length)
        {
            int number;
            string nonce = String.Empty;     //存放随机码的字符串   

            System.Random random = new Random();

            for (int i = 0; i < length; i++) //产生4位校验码   
            {
                number = random.Next();
                number = number % 36;
                if (number < 10)
                {
                    number += 48;    //数字0-9编码在48-57   
                }
                else
                {
                    number += 55;    //字母A-Z编码在65-90   
                }

                nonce += ((char)number).ToString();
            }
            return nonce;
        }

        public long GetTimeStamp()
        {
            var baseTime = Convert.ToDateTime("1970-1-1 0:00:00");
            var ts = DateTime.Now - baseTime;
            long intervel = (long)ts.TotalMilliseconds / 1000;
            return intervel;
        }

        public string SHA1Hash(string plain)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(plain));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    // can be "x2" if you want lowercase
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }
        }
    }
}
