using EventBus;
using Microsoft.AspNetCore.Mvc;
using SearchService.Domain;
using SearchService.Domain.Entities;
using SearchService.WebAPI.Request;

namespace SearchService.WebAPI.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class SearchController : ControllerBase
{

    private readonly IEpisodeSearchRepository episodeSearch;
    private readonly IOrderSearchRepository orderSearch;
    private readonly IEventBus eventBus;

    public SearchController(IEpisodeSearchRepository episodeSearch,IOrderSearchRepository orderSearch ,IEventBus eventBus)
    {
        this.episodeSearch = episodeSearch;
        this.orderSearch = orderSearch;
        this.eventBus = eventBus;
    }

    [HttpGet]
    public Task<SearchResult<Episode>> SearchEpisodes([FromQuery] SearchEpisodesRequest req)
    {
        return episodeSearch.SearchAsync(req.Keyword, req.PageIndex, req.PageSize);
    }

    [HttpPost]
    public async Task<ActionResult> Test(Guid Id)
    {
        //eventBus.Publish("OrderTable.Created", new { TableId =  Guid.NewGuid() , OrderId = Guid.NewGuid(), TableName="jbvdfvn", ProjectText="dksjlnvjdsfnjd" });
        eventBus.Publish("OrderService.Order.Update", new { OrderId= Id, ProjectText = "yepeng" });
        return Ok();
    }
    [HttpGet]
    public Task<SearchResult<Order>> SearchOrders([FromQuery] SearchOrdersRequest req)
    {
        return orderSearch.SearchAsync(req.Keyword, req.PageIndex, req.PageSize);
    }

    [HttpPut]
    public async Task<IActionResult> ReIndexAll()
    {
        //避免耦合，这里发送ReIndexAll的集成事件
        //所有向搜索系统贡献数据的系统都可以响应这个事件，重新贡献数据
        eventBus.Publish("SearchService.ReIndexAll", null);
        return Ok();
    }
}
