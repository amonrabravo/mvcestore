using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MVCEStoreWeb
{
    public static class Extensions
    {
        public static string ToSafeUrlString(this string Text) => Regex.Replace(string.Concat(Text.Where(p => char.IsLetterOrDigit(p) || char.IsWhiteSpace(p))), @"\s+", "-").ToLower();
    }
}
