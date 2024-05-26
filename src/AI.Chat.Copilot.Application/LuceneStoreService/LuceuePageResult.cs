using Lucene.Net.Documents;
using Lucene.Net.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI.Chat.Copilot.Application.LuceneStoreService
{
    public class LuceuePageResult
    {
        public Document[] Docs { get; set; }
        public ScoreDoc ScoreDoc { get; set; }
    }
}
