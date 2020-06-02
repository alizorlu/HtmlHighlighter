using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsuiteNetProxyDemo.Extentions
{
    public static class StringExtentions
    {
        public static string EditSourceLink(this string value, string pageUri)
        {
            if (value.Contains(":")) return value; //'whatsapp://','mailto:','http:','https:','data:image'
            if (value.StartsWith("//")) return value; // '//cdn.xxx.com'
            if (value.StartsWith("#")) return value; // '#'
            else
            {
                var url = new Uri(pageUri);
                string attributeLinkFormat = $"{url.Scheme}://{url.Host}{value}";
                return attributeLinkFormat;
            }
        }
        public static string IsDomainOrSubdomain(this string value)
        {
            return value;
        }
        public static string ReplaceSpecialChars(this string value)
        {
            value = value.Replace("\t", "").Replace("\n", "").Replace("\r", "").Trim();
            return value;
        }
    }
}
