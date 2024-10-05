namespace eBid.Search.API.Repositories.Caching;

public class CachingElasticSearchRepository(
    IElasticSearchRepository<AuctionItemData> repository,
    RedisService redisService)
    : IElasticSearchRepository<AuctionItemData>
{
    public async Task<List<AuctionItemData>> GetAllItems(string index, int from, int size)
    {
        return await repository.GetAllItems(index, from, size);
    }

    public async Task<AuctionItemData> GetItemById(string index, int id)
    {
        return await repository.GetItemById(index, id);
    }

    public async Task<List<AuctionItemData>> GetItemsByName(string index, string term, int from, int size)
    {
        return await repository.GetItemsByName(index, term, from, size);
    }

    public async Task<List<AuctionItemData>> GetItemsByTypeId(string index, int typeId, int from, int size)
    {
        return await repository.GetItemsByTypeId(index, typeId, from, size);
    }

    public async Task<List<AuctionItemData>> GetItemsByIds(string index, int[] ids, int from, int size)
    {
        return await repository.GetItemsByIds(index, ids, from, size);
    }

    public async Task<List<AuctionItemData>> GetItemsByStatus(string index, string status, int from, int size)
    {
        return await repository.GetItemsByStatus(index, status, from, size);
    }

    public async Task<List<(int, string)>> GetTypes(string index, int from, int size)
    {
        return await repository.GetTypes(index, from, size);
    }

    public async Task<bool> GetItemIsOnSell(string index, int id)
    {
        return await repository.GetItemIsOnSell(index, id);
    }

    public async Task<List<AuctionItemData>> GetItemsBySub(string index, string sub, int from, int size)
    {
        return await repository.GetItemsBySub(index, sub, from, size);
    }
}