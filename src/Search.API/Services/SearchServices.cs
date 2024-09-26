namespace eBid.Search.API.Services;

public class SearchServices(
    ILogger<SearchServices> logger,
    IElasticSearchRepository<AuctionItemData> esRepository)
{
    public IElasticSearchRepository<AuctionItemData> EsRepository { get; } = esRepository;
    public ILogger<SearchServices> Logger { get; } = logger;
}