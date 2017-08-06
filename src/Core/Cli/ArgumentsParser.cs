using System.Collections.Generic;
using System.Linq;

namespace ComKit.Core.Cli
{
    public class ArgumentsParser
    {
        readonly List<string> flags = new List<string>();

        public void Parse(string[] args)
        {
            foreach(var a in args)
            {
                if (a.StartsWith("/", System.StringComparison.Ordinal) || a.StartsWith("-", System.StringComparison.Ordinal))
                {
                    var f = a.Substring(1);

                    var fs = f.ToCharArray().Select(x => x.ToString());

                    flags.AddRange(fs);
                }
            }
        }

        public bool CheckFlag(string flag)
        {
            return flags.Contains(flag);
        }
    }
}