using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikipediaActivity
{
    public class Continue
    {
        public string rccontinue { get; set; }
        public string @continue { get; set; }
    }

    public class Limits
    {
        public int recentchanges { get; set; }
    }

    public class Recentchange
    {
        public string type { get; set; }
        public int ns { get; set; }
        public string title { get; set; }
        public int oldlen { get; set; }
        public int newlen { get; set; }
        public DateTime timestamp { get; set; }
    }

    public class Query
    {
        public List<Recentchange> recentchanges { get; set; }
    }

    public class RootObject
    {
        public string batchcomplete { get; set; }
        public Continue @continue { get; set; }
        public Limits limits { get; set; }
        public Query query { get; set; }
    }
}
