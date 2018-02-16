using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikipediaActivity
{
    class ArticleData
    {
        //TODO: Add metric for the number of users who edited the article/talk page.
        public string Name { get; private set; }
        public int NumberOfChanges { get; private set; }
        public int ChangesDelta { get; private set; }
        public int NumberOfTalkChanges { get; private set; }
        public int TalkChangesDelta { get; private set; }
        public bool InNotabilityCategory { get; set; }
        public bool InDeletionCategory { get; set; }

        public ArticleData(string name, int numberOfChanges, int changesDelta, int numberOfTalkChanges, int talkChangesDelta)
        {
            this.Name = name;
            this.NumberOfChanges = numberOfChanges;
            this.ChangesDelta = changesDelta;
            this.NumberOfTalkChanges = numberOfTalkChanges;
            this.TalkChangesDelta = talkChangesDelta;
        }

        public override string ToString()
        {
            return this.Name;
        }

        public int Score
        {
            get
            {
                int score = 0;
                const int CHANGE_COUNT_SCORE = 20;
                const int CHANGE_DELTA_SCORE = 1;
                const int TALK_COUNT_SCORE = 40;
                const int TALK_DELTA_SCORE = 5;
                const int NOTABILITY_SCORE = 100;
                const int DELETION_SCORE = 100;

                score += this.NumberOfChanges * CHANGE_COUNT_SCORE;
                score += this.ChangesDelta * CHANGE_DELTA_SCORE;
                score += this.NumberOfTalkChanges * TALK_COUNT_SCORE;
                score += this.TalkChangesDelta * TALK_DELTA_SCORE;
                score += Convert.ToInt32(InNotabilityCategory) * NOTABILITY_SCORE;
                score += Convert.ToInt32(InDeletionCategory) * DELETION_SCORE;

                return score;
            }
        }
    }
}
