using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace WikipediaActivity
{
    class Program
    {
        static string GetKeyFromTitle(RecentChanges.Recentchange recentchange)
        {
            if (recentchange.title.StartsWith("שיחה:"))
            {
                return recentchange.title.Substring(5);
            }
            return recentchange.title;
        }
        static List<ArticleData> GetDayHistory(string baseUri)
        {
            string resource = "/w/api.php?action=query&format=json&list=recentchanges&utf8=1&rcnamespace=1|0&rcprop=title|sizes|timestamp&rclimit=max";
            IRestResponse<RecentChanges.RootObject> result = ExecuteRecentChangesQuery(baseUri, resource);
            List<RecentChanges.Recentchange> recentChanges = result.Data.query.recentchanges;

            while (result.Data.query.recentchanges.Last().timestamp.ToLocalTime() > DateTime.Now.Subtract(new TimeSpan(24, 0, 0)))
            {
                // need to execute another query
                string rccontinue = result.Data.@continue.rccontinue;
                result = ExecuteRecentChangesQuery(baseUri, resource + "&rccontinue=" + rccontinue);
                recentChanges.AddRange(result.Data.query.recentchanges);
            }

            var groupedRecentChanges = recentChanges.GroupBy(GetKeyFromTitle);
            List<ArticleData> articles = new List<ArticleData>();

            foreach (var group in groupedRecentChanges)
            {
                var talkEntries = group.Where(x => x.title.StartsWith("שיחה:")).ToList();
                var nonTalkEntries = group.Where(x => !x.title.StartsWith("שיחה:")).ToList();

                int talkChange = talkEntries.Sum(x => Math.Abs(x.newlen - x.oldlen));
                int nontTalkChange = nonTalkEntries.Sum(x => Math.Abs(x.newlen - x.oldlen));
                int talkCount = talkEntries.Count();
                int mainCount = nonTalkEntries.Count();
                string name = group.Key;

                ArticleData article = new ArticleData(name, nontTalkChange, mainCount, talkCount, talkChange);
                articles.Add(article);

            }
            return articles;
        }

        private static IRestResponse<RecentChanges.RootObject> ExecuteRecentChangesQuery(string baseUri, string resource)
        {
            var client = new RestClient(baseUri);
            var request = new RestRequest(resource, Method.GET);
            IRestResponse<RecentChanges.RootObject> result = client.Execute<RecentChanges.RootObject>(request);
            return result;
        }

        static void Main(string[] args)
        {
            GetDayHistory("https://he.wikipedia.org");
        }
    }
}
