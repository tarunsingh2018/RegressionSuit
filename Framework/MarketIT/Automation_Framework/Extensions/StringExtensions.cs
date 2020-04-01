using System;

namespace Framework.MarketIT.Automation_Framework.Extensions
{
    public static class StringExtensions
    {
        public static bool ContainsIgnoreCase(this string s,string subString)
        {
            if (s == null || subString == null) return false;
            return s.IndexOf(subString, StringComparison.OrdinalIgnoreCase) != -1;
        }
    }
}
