using System.Collections.Generic;
using System.Linq;

namespace GrammaGo.Server.Data
{
    public static class StringExtensions
    {
        public static List<string> SplitToList(this string source, char separator)
        {
            return source.Split(separator).ToList();
        }
    }
}
