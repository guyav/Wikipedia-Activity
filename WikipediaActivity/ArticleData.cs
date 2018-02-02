using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikipediaActivity
{
    class ArticleData
    {
        string name;
        int numberOfChanges;
        int changesDelta;
        int numberOfTalkChanges;
        int talkChangesDelta;

        public ArticleData(string name, int numberOfChanges, int changesDelta, int numberOfTalkChanges, int talkChangesDelta)
        {
            this.name = name;
            this.numberOfChanges = numberOfChanges;
            this.changesDelta = changesDelta;
            this.numberOfTalkChanges = numberOfTalkChanges;
            this.talkChangesDelta = talkChangesDelta;
        }
    }
}
