namespace eBid.Search.API.Services;

public class RedisService(ILogger<RedisService> logger, IConnectionMultiplexer redis)
{
    private readonly IDatabase _database = redis.GetDatabase();

    // implementations:

    // - /auction/{id} "string" per unique auction
    private static RedisKey ItemKeyPrefix = "/items/"u8.ToArray();
    // note on UTF8 here: library limitation (to be fixed) - prefixes are more efficient as blobs

    private static RedisKey GeKey(string auctionId) => ItemKeyPrefix.Append(auctionId);

    public async Task<bool> DeleteItemAsync(int id)
    {
        return await _database.KeyDeleteAsync(GeKey(id.ToString()));
    }

    public async Task<AuctionItemData> GetItemAsync(int itemId)
    {
        using var data = await _database.StringGetLeaseAsync(GeKey(itemId.ToString()));

        if (data is null || data.Length == 0)
        {
            return null;
        }

        return JsonSerializer.Deserialize(data.Span, AuctionSerializationContext.Default.AuctionItemData);
    }

    public async Task<IEnumerable<AuctionItemData>?> GetItemsAsync(int[] ids)
    {
        var items = new List<AuctionItemData>();

        foreach (var id in ids)
        {
            var item = await GetItemAsync(id);
            if (item != null)
            {
                items.Add(item);
            }
        }

        return items.Count == 0 ? null : items;
    }


    public async Task UpdateItemsAsync(IEnumerable<AuctionItemData> items)
    {
        foreach (var item in items)
        {
            await UpdateItemAsync(item);
        }

        logger.LogInformation("Items persisted successfully.");
    }


    public async Task UpdateItemAsync(AuctionItemData auction)
    {
        var json = JsonSerializer.SerializeToUtf8Bytes(auction, AuctionSerializationContext.Default.AuctionItemData);
        var created =
            await _database.StringSetAsync(GeKey(auction.Id.ToString()), json, expiry: TimeSpan.FromSeconds(60));

        if (!created)
        {
            logger.LogInformation("Problem occurred persisting the item.");
            return;
        }

        logger.LogInformation("Item persisted successfully.");
    }
}

[JsonSerializable(typeof(AuctionItemData))]
[JsonSourceGenerationOptions(PropertyNameCaseInsensitive = true)]
public partial class AuctionSerializationContext : JsonSerializerContext
{
}