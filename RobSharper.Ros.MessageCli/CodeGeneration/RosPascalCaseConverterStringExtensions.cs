using System;
using System.Linq;

namespace RobSharper.Ros.MessageCli.CodeGeneration
{
    public static class RosPascalCaseConverterStringExtensions
    {
        public static string ToPascalCase(this string name)
        {
            if (name == null)
                return null;
         
            name = name
                .Split(new [] {"_"}, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => char.ToUpperInvariant(s[0]) + s.Substring(1, s.Length - 1))
                .Aggregate(string.Empty, (s1, s2) => s1 + s2);

            return name;
        }
    }
}