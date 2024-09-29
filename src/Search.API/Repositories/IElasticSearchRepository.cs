namespace eBid.Search.API.Repositories;

public interface IElasticSearchRepository<TEntity>
{
    Task<List<TEntity>> GetAllItems(string index, int from, int size);
    Task<TEntity> GetItemById(string index, int id);
    Task<List<TEntity>> GetItemsByName(string index, string term, int from, int size);
    Task<List<TEntity>> GetItemsByTypeId(string index, int typeId, int from, int size);
    Task<List<TEntity>> GetItemsByIds(string index, int[] ids, int from, int size);
    Task<List<TEntity>> GetItemsByStatus(string index, string status, int from, int size);
    Task<List<(int, string)>> GetTypes(string index, int from, int size);
    Task<bool> GetItemIsOnSell(string index, int id);
    Task<List<TEntity>> GetItemsBySub(string index, string sub, int from, int size);
}