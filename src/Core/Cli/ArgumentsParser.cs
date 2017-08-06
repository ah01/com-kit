using System.Collections.Generic;

namespace ComKit.Core.Cli
{
    public class ArgumentsParser
    {
        private List<string> flags = new List<string>();

        public ArgumentsParser()
        {
        }

        public void Parse(string[] args)
        {
            foreach(var a in args)
            {
                if (a.StartsWith("/") || a.StartsWith("-"))
                {
                    var f = a.Substring(1);
                    flags.Add(f);
                }
            }
        }

        public bool CheckFlag(string flag)
        {
            return flags.Contains(flag);
        }


    }
}