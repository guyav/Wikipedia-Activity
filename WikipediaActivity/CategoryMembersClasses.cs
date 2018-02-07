using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikipediaActivity.CategoryMembers
{
    public class Limits
    {
        public int categorymembers { get; set; }
    }

    public class Categorymember
    {
        public int ns { get; set; }
        public string title { get; set; }
    }

    public class Query
    {
        public List<Categorymember> categorymembers { get; set; }
    }

    public class RootObject
    {
        public string batchcomplete { get; set; }
        public Limits limits { get; set; }
        public Query query { get; set; }
    }
}
