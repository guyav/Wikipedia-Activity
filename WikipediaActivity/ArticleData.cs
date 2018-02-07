using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikipediaActivity
{
    class ArticleData
    {
        public string name { get; private set; }
        public int numberOfChanges { get; private set; }
        public int changesDelta { get; private set; }
        public int numberOfTalkChanges { get; private set; }
        public int talkChangesDelta { get; private set; }
        public bool inNotabilityCategory { get; set; }
        public bool inDeletionCategory { get; set; }

        public ArticleData(string name, int numberOfChanges, int changesDelta, int numberOfTalkChanges, int talkChangesDelta)
        {
            this.name = name;
            this.numberOfChanges = numberOfChanges;
            this.changesDelta = changesDelta;
            this.numberOfTalkChanges = numberOfTalkChanges;
            this.talkChangesDelta = talkChangesDelta;
        }

        public override string ToString()
        {
            return this.name;
        }
    }
}
