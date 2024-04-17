using Nest;
using SearchService.Domain;
using SearchService.Domain.Entities;

namespace SearchService.Infrastructure.Repository;

public class EpisodeSearchRepository : IEpisodeSearchRepository
{
    private readonly IElasticClient elasticClient;

    public EpisodeSearchRepository(IElasticClient elasticClient)
    {
        this.elasticClient = elasticClient;
    }

    public Task DeleteAsync(Guid episodeId)
    {
        elasticClient.DeleteByQuery<Episode>(q => q
           .Index("episodes")
           .Query(rq => rq.Term(f => f.Id, "elasticsearch.pm")));
        //因为有可能文档不存在，所以不检查结果
        //如果Episode被删除，则把对应的数据也从Elastic Search中删除
        return elasticClient.DeleteAsync(new DeleteRequest("episodes", episodeId));
    }

    public async Task<SearchResult<Episode>> SearchAsync(string Keyword, int PageIndex, int PageSize)
    {
        int from = PageSize * (PageIndex - 1);
        string kw = Keyword;
        Func<QueryContainerDescriptor<Episode>, QueryContainer> query = (q) =>
                      q.Match(mq => mq.Field(f => f.CnName).Query(kw))
                      || q.Match(mq => mq.Field(f => f.EngName).Query(kw))
                      || q.Match(mq => mq.Field(f => f.PlainSubtitle).Query(kw));
        Func<HighlightDescriptor<Episode>, IHighlight> highlightSelector = h => h
            .Fields(fs => fs.Field(f => f.PlainSubtitle));
        var result = await elasticClient.SearchAsync<Episode>(s => s.Index("episodes").From(from)
            .Size(PageSize).Query(query).Highlight(highlightSelector));
        if (!result.IsValid)
        {
            throw result.OriginalException;
        }
        List<Episode> episodes = new List<Episode>();
        foreach (var hit in result.Hits)
        {
            string highlightedSubtitle;
            //如果没有预览内容，则显示前50个字
            if (hit.Highlight.ContainsKey("plainSubtitle"))
            {
                highlightedSubtitle = string.Join("\r\n", hit.Highlight["plainSubtitle"]);
            }
            else
            {
                highlightedSubtitle = hit.Source.PlainSubtitle.Cut(50);
            }
            var episode = hit.Source with { PlainSubtitle = highlightedSubtitle };
            episodes.Add(episode);
        }
        return new SearchResult<Episode>(episodes, result.Total);
    }

    public async Task UpdateAsync(Guid episodeId, Dictionary<string, string> updatedFields)
    {
        var existingOrder = await elasticClient.GetAsync<Episode>(episodeId, idx => idx.Index("episodes"));
        if (existingOrder.Source != null)
        {
            foreach (var field in updatedFields)
            {
                existingOrder.Source.GetType().GetProperty(field.Key)?.SetValue(existingOrder.Source, field.Value);
            }

            var response = await elasticClient.IndexAsync(existingOrder.Source, idx => idx.Index("episodes").Id(episodeId));
            if (!response.IsValid)
            {
                throw new ApplicationException(response.DebugInformation);
            }
        }
        else
        {
            throw new ApplicationException("Order not found in Elasticsearch.");
        }
    }

    public async Task UpsertAsync(Episode episode)
    {
        var response = await elasticClient.IndexAsync(episode, idx => idx.Index(nameof(Episode).ToLower()).Id(episode.Id));//Upsert:Update or Insert
        if (!response.IsValid)
        {
            throw new ApplicationException(response.DebugInformation);
        }
    }

}
