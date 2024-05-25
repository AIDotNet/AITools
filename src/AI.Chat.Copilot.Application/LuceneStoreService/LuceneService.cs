using AI.Chat.Copilot.Application.LuceneStoreService;
using Azure.AI.OpenAI;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI.Chat.Copilot.Application
{
    public class LuceneService
    {
        private static readonly string BasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"index");
        const LuceneVersion AppLuceneVersion = LuceneVersion.LUCENE_48;

        public void WriteDocs(string categoryName, IEnumerable<Document> documents)
        {
            using var dir = FSDirectory.Open(Path.Combine(BasePath,categoryName));
            using var analyzer = new StandardAnalyzer(AppLuceneVersion);
            var indexConfig = new IndexWriterConfig(AppLuceneVersion, analyzer);
            using var writer = new IndexWriter(dir, indexConfig);
            writer.AddDocuments(documents);
            writer.Commit();
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        public LuceuePageResult PaginationQuery(string categoryName, Query query, ScoreDoc scoreDoc = null)
        {
            using var dir = FSDirectory.Open(Path.Combine(BasePath, categoryName));
            using DirectoryReader reader = DirectoryReader.Open(dir);
            var searcher = new IndexSearcher(DirectoryReader.Open(dir));

            TopDocs initialResults = scoreDoc == null ? searcher.Search(query, 10) : searcher.SearchAfter(scoreDoc, query, 10);
            ScoreDoc lastScoreDoc = initialResults.ScoreDocs?.LastOrDefault();
            return new LuceuePageResult
            {
                Docs = initialResults,
                ScoreDoc = lastScoreDoc
            };
        }
    }
}
