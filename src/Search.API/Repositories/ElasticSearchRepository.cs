using System.Data;

using Elastic.Clients.Elasticsearch.QueryDsl;

namespace eBid.Search.API.Repositories;

public class ElasticSearchRepository(ElasticsearchClient client)
    : IElasticSearchRepository<AuctionItemData>
{
    public async Task<List<AuctionItemData>> GetAllItems(string index, int from, int size)
    {
        var response = await client.SearchAsync<AuctionItemData>(s =>
            s.Index(index)
                .From(from)
                .Size(size)
        );

        if (response.IsValidResponse)
        {
            return response.Documents.ToList();
        }

        return [];
    }

    public async Task<AuctionItemData> GetItemById(string index, int id)
    {
        var response = await client.SearchAsync<AuctionItemData>(s => s
            .Index(index)
            .Query(q => q
                .Match(m => m
                    .Field(data => data.Id)
                    .Query(id)
                )
            )
        );

        if (response.IsValidResponse)
        {
            return response.Documents.FirstOrDefault();
        }

        return null;
    }

    public async Task<List<AuctionItemData>> GetItemsByName(string index, string term, int from, int size)
    {
        var response = await client.SearchAsync<AuctionItemData>(s =>
            s.Index(index).From(from).Size(size)
                .Query(q => q
                    .Match(m => m
                        .Field(data => data.Name)
                        .Query(term)
                    )
                )
        );

        return response.IsValidResponse ? response.Documents.ToList() : [];
    }

    public async Task<List<AuctionItemData>> GetItemsByIds(string index, int[] ids, int from, int size)
    {
        if (ids == null || ids.Length == 0)
        {
            return [];
        }

        var response = await client.SearchAsync<AuctionItemData>(s => s
            .Index(index)
            .Query(q => q
                .Ids(i => i
                    .Values(new Ids(ids.Select(id => id.ToString())))
                )
            )
            .From(from)
            .Size(size)
        );

        if (!response.IsValidResponse)
        {
            return [];
        }

        return response.Documents.ToList();
    }

    public async Task<List<AuctionItemData>> GetItemsByTypeId(string index, int typeId, int from, int size)
    {
        var response = await client.SearchAsync<AuctionItemData>(s =>
        {
            s.Index(index)
                .Query(q => q.Match(m =>
                    m.Field(data => data.AuctionTypeId).Query(typeId)))
                .From(from).Size(size);
        });

        return response.IsValidResponse ? response.Documents.ToList() : [];
    }

    public async Task<List<AuctionItemData>> GetItemsByStatus(string index, string status, int from, int size)
    {
        var response = await client.SearchAsync<AuctionItemData>(s =>
        {
            s.Index(index)
                .Query(q => q.Match(m =>
                    m.Field(data => data.Status)
                        .Query(status.ToString())))
                .From(from).Size(size);
        });

        if (response.IsValidResponse)
        {
            return response.Documents.ToList();
        }

        return [];
    }

    public async Task<List<(int, string)>> GetTypes(string index, int from, int size)
    {
        var hasIndex = await client.Indices.ExistsAsync(index);

        if (!hasIndex.Exists)
        {
            return [];
        }

        var response = await client.SearchAsync<AuctionItemData>(s => s
            .Index(index)
            .From(from)
            .Size(size)
        );

        var result = response.Documents
            .Select(data => (data.AuctionTypeId, data.AuctionType))
            .Distinct()
            .ToList();

        return result;
    }

    public async Task<bool> GetItemIsOnSell(string index, int id)
    {
        if (!(await client.Indices.ExistsAsync(index)).Exists)
        {
            throw new DataException("Index not found");
        }

        var response = await client.SearchAsync<AuctionItemData>(s => s
            .Index(index)
            .Query(q => q
                .Match(m => m
                    .Field(data => data.Id)
                    .Query(id)
                )
            )
        );

        if (response.IsValidResponse)
        {
            return response.Documents.FirstOrDefault()?.OnSell ?? false;
        }

        throw new DataException("Item not found");
    }

    public async Task<List<AuctionItemData>> GetItemsBySub(string index, string sub, int from, int size)
    {
        var hasIndex = await client.Indices.ExistsAsync(index);

        if (!hasIndex.Exists)
        {
            return [];
        }

        var response = await client.SearchAsync<AuctionItemData>(s =>
            s.Index(index).From(from).Size(size)
                .Query(q => q
                    .Match(m => m
                        .Field(data => data.SellerId)
                        .Query(sub)
                    )
                )
        );

        return response.IsValidResponse ? response.Documents.ToList() : [];
    }
}