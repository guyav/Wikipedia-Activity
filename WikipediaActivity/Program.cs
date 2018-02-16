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

        static List<string> ArticlesInCategory(string baseUri, string category)
        {
            string resource = string.Format("/w/api.php?action=query&format=json&list=categorymembers&utf8=1&cmtitle={0}&cmprop=title&cmnamespace=0&cmlimit=max", category);
            IRestResponse<CategoryMembers.RootObject> result = ExecuteCategoryMembersQuery(baseUri, resource);
            List<CategoryMembers.Categorymember> categoryMembers = result.Data.query.categorymembers;
            return categoryMembers.Select(x => x.title).ToList();
        }

        private static IRestResponse<RecentChanges.RootObject> ExecuteRecentChangesQuery(string baseUri, string resource)
        {
            var client = new RestClient(baseUri);
            var request = new RestRequest(resource, Method.GET);
            IRestResponse<RecentChanges.RootObject> result = client.Execute<RecentChanges.RootObject>(request);
            return result;
        }

        private static IRestResponse<CategoryMembers.RootObject> ExecuteCategoryMembersQuery(string baseUri, string resource)
        {
            var client = new RestClient(baseUri);
            var request = new RestRequest(resource, Method.GET);
            IRestResponse<CategoryMembers.RootObject> result = client.Execute<CategoryMembers.RootObject>(request);
            return result;
        }

        static void Main(string[] args)
        {
            string baseUri = "https://he.wikipedia.org";
            List<ArticleData> historyArticles = GetDayHistory(baseUri);

            List<string> notabilityArticles = ArticlesInCategory(baseUri, "קטגוריה:ויקיפדיה: ערכים שיש להבהיר את מעמדם");
            foreach(string notabilityArticle in notabilityArticles)
            {
                ArticleData foundArticle = historyArticles.Find(x => x.Name == notabilityArticle);
                if (foundArticle==null)
                {
                    continue;
                }
                foundArticle.InNotabilityCategory = true;
            }
            List<string> deletionArticles = ArticlesInCategory(baseUri, "קטגוריה:ויקיפדיה: הצבעות מחיקה");
            foreach (string deletionArticle in deletionArticles)
            {
                ArticleData foundArticle = historyArticles.Find(x => x.Name == deletionArticle);
                if (foundArticle == null)
                {
                    continue;
                }
                foundArticle.InDeletionCategory = true;
            }
        }
    }
}
