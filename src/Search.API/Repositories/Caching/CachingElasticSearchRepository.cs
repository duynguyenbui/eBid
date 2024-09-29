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
        var cache = await redisService.GetItemAsync(id);

        if (cache != null)
        {
            Console.WriteLine($"Item found in cache with /auction/{id}");
            return cache;
        }

        var item = await repository.GetItemById(index, id);
        if (item != null) await redisService.UpdateItemAsync(item);

        return item;
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
        var cache = await redisService.GetItemsAsync(ids);

        if (cache != null)
        {
            Console.WriteLine($"Items found in cache with /auctions/ids");
            return cache.ToList();
        }

        var items = await repository.GetItemsByIds(index, ids, from, size);
        await redisService.UpdateItemsAsync(items);

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