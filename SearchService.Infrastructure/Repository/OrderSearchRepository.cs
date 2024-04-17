using Nest;
using SearchService.Domain;
using SearchService.Domain.Entities;

namespace SearchService.Infrastructure.Repository;

public class OrderSearchRepository : IOrderSearchRepository
{
    private readonly IElasticClient elasticClient;

    public OrderSearchRepository(IElasticClient elasticClient)
    {
        this.elasticClient = elasticClient;
    }

    public async Task UpsertAsync(Order order)
    {
        var response = await elasticClient.IndexAsync(order, idx => idx.Index(nameof(Order).ToLower()).Id(order.OrderId));//Upsert:Update or Insert
        if (!response.IsValid)
        {
            throw new ApplicationException(response.DebugInformation);
        }
    }

    public Task DeleteAsync(Guid orderId)
    {
        elasticClient.DeleteByQuery<Order>(q => q
           .Index(nameof(Order).ToLower())
           .Query(rq => rq.Term(f => f.OrderId, "elasticsearch.pm")));
        return elasticClient.DeleteAsync(new DeleteRequest(nameof(Order).ToLower(), orderId));
    }

    public async Task UpdateAsync(Guid orderId, Dictionary<string, string> updatedFields)
    {
        var existingOrder = await elasticClient.GetAsync<Order>(orderId, idx => idx.Index(nameof(Order).ToLower()));
        /*var searchResponse = await elasticClient.SearchAsync<Order>(s => s
        .Index(nameof(Order).ToLower()).Query(q => q.Match(m => m.Field(f => f.OrderId).Query(orderId.ToString()))));*/

        if (existingOrder.Source != null)
        {
            foreach (var field in updatedFields)
                existingOrder.Source.GetType().GetProperty(field.Key)?.SetValue(existingOrder.Source, field.Value);

            var response = await elasticClient.IndexAsync(existingOrder.Source, idx => idx.Index(nameof(Order).ToLower()).Id(orderId));
            if (!response.IsValid)
                throw new ApplicationException(response.DebugInformation);
        }
        else
            throw new ApplicationException("Order not found in Elasticsearch.");
    }

    public async Task<SearchResult<Order>> SearchAsync(string keyWord, int pageIndex, int PageSize)
    {
        int from = PageSize * (pageIndex - 1);

        Func<QueryContainerDescriptor<Order>, QueryContainer> query = q => q.Match(mq => mq.Field(f => f.ProjectText).Query(keyWord));
        Func<HighlightDescriptor<Order>, IHighlight> highlightSelector = q => q.Fields(f => f.Field(f => f.ProjectText));

        var res = await elasticClient.SearchAsync<Order>(s => s.Index(nameof(Order).ToLower()).From(from)
            .Size(PageSize).Query(query).Highlight(highlightSelector));
        
        if (!res.IsValid)
            throw res.OriginalException;
        List<Order> orders = [];
        
        foreach (var hit in res.Hits)
        {
            string highlightedSubtitle;
            if (hit.Highlight.ContainsKey("projectText"))
                highlightedSubtitle = string.Join("\r\n", hit.Highlight["projectText"]);
            else
                highlightedSubtitle = hit.Source.ProjectText.Cut(50);
            var order = hit.Source with { ProjectText = highlightedSubtitle };
            orders.Add(order);
        }
        return new SearchResult<Order>(orders, res.Total);
    }
}
